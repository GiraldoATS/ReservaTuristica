using System.Reflection;
using FondoXYZ.Application.Common;
using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.Entities;
using FondoXYZ.Domain.Enums;
using FondoXYZ.Domain.ValueObjects;

namespace FondoXYZ.Tests.Application;

// Implementaciones en memoria de los puertos de salida. Gracias a la arquitectura
// hexagonal, los casos de uso se prueban sin base de datos ni librerías de mocking.

internal static class Ids
{
    /// <summary>Simula la asignación de identidad que haría la base de datos.</summary>
    public static T Asignar<T>(T entidad, string propiedad, int id)
    {
        typeof(T).GetProperty(propiedad, BindingFlags.Public | BindingFlags.Instance)!.SetValue(entidad, id);
        return entidad;
    }
}

internal sealed class FakeTimeProvider : TimeProvider
{
    private readonly DateTimeOffset _ahora;

    public FakeTimeProvider(DateTime ahora)
    {
        _ahora = new DateTimeOffset(ahora, TimeSpan.Zero);
    }

    public override DateTimeOffset GetUtcNow() => _ahora;

    public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
}

internal sealed class FakeUnitOfWork : IUnitOfWork
{
    public int VecesGuardado { get; private set; }

    public Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default)
    {
        VecesGuardado++;
        return Task.FromResult(1);
    }
}

internal sealed class FakeSedeRepository : ISedeRepository
{
    public List<Sede> Sedes { get; } = [];

    public Task<IReadOnlyList<Sede>> ObtenerTodasAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Sede>>(Sedes);

    public Task<bool> ExisteAsync(int idSede, CancellationToken cancellationToken = default) =>
        Task.FromResult(Sedes.Any(s => s.IdSede == idSede));

    public void Agregar(Sede sede) => Sedes.Add(sede);
}

internal sealed class FakeAlojamientoRepository : IAlojamientoRepository
{
    public List<Alojamiento> Alojamientos { get; } = [];

    public Task<IReadOnlyList<Alojamiento>> ObtenerTodosAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Alojamiento>>(Alojamientos);

    public Task<Alojamiento?> ObtenerPorIdAsync(int idAlojamiento, CancellationToken cancellationToken = default) =>
        Task.FromResult(Alojamientos.FirstOrDefault(a => a.IdAlojamiento == idAlojamiento));

    public Task<bool> ExisteAsync(int idAlojamiento, CancellationToken cancellationToken = default) =>
        Task.FromResult(Alojamientos.Any(a => a.IdAlojamiento == idAlojamiento));

    public void Agregar(Alojamiento alojamiento) => Alojamientos.Add(alojamiento);

    public void Eliminar(Alojamiento alojamiento) => Alojamientos.Remove(alojamiento);
}

internal sealed class FakeTarifaRepository : ITarifaRepository
{
    public List<Tarifa> Tarifas { get; } = [];

    public Task<IReadOnlyList<Tarifa>> ObtenerTodasAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Tarifa>>(Tarifas);

    public Task<Tarifa?> ObtenerPorIdAsync(int idTarifa, CancellationToken cancellationToken = default) =>
        Task.FromResult(Tarifas.FirstOrDefault(t => t.IdTarifa == idTarifa));

    public Task<bool> ExisteParaAlojamientoAsync(int idAlojamiento, CancellationToken cancellationToken = default) =>
        Task.FromResult(Tarifas.Any(t => t.IdAlojamiento == idAlojamiento));

    public void Agregar(Tarifa tarifa) => Tarifas.Add(tarifa);

    public void Eliminar(Tarifa tarifa) => Tarifas.Remove(tarifa);
}

internal sealed class FakeReservaRepository : IReservaRepository
{
    public List<Reserva> Reservas { get; } = [];

    public Task<IReadOnlyList<Reserva>> ObtenerTodasAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Reserva>>(Reservas);

    public Task<bool> ExisteCruceAsync(int idAlojamiento, RangoFechas periodo, CancellationToken cancellationToken = default) =>
        Task.FromResult(Reservas.Any(r =>
            r.IdAlojamiento == idAlojamiento
            && r.Estado == EstadoReserva.Activa
            && r.Periodo.SeCruzaCon(periodo)));

    public Task<bool> ExisteParaAlojamientoAsync(int idAlojamiento, CancellationToken cancellationToken = default) =>
        Task.FromResult(Reservas.Any(r => r.IdAlojamiento == idAlojamiento));

    public void Agregar(Reserva reserva) => Reservas.Add(reserva);
}

internal sealed class FakeDisponibilidadQuery : IDisponibilidadQuery
{
    public RangoFechas? UltimoPeriodoConsultado { get; private set; }

    public Task<IReadOnlyList<DisponibilidadDto>> ConsultarAsync(RangoFechas periodo, CancellationToken cancellationToken = default)
    {
        UltimoPeriodoConsultado = periodo;
        return Task.FromResult<IReadOnlyList<DisponibilidadDto>>([]);
    }
}

internal sealed class FakeCalculoTarifaQuery : ICalculoTarifaQuery
{
    /// <summary>Precio por noche a cobrar; null simula que no hay tarifa vigente.</summary>
    public decimal? PrecioPorNoche { get; set; } = 100_000m;

    public Task<CalculoTarifaDto?> CalcularAsync(
        int idAlojamiento,
        int numeroPersonas,
        RangoFechas periodo,
        CancellationToken cancellationToken = default)
    {
        if (PrecioPorNoche is null)
            return Task.FromResult<CalculoTarifaDto?>(null);

        var total = PrecioPorNoche.Value * periodo.Noches;

        return Task.FromResult<CalculoTarifaDto?>(
            new CalculoTarifaDto(1, PrecioPorNoche.Value, 0, periodo.Noches, 0, total));
    }
}

internal sealed class FakeIdentityService : IIdentityService
{
    public Dictionary<string, string> TokensPorEmail { get; } = [];

    public Task<bool> IniciarSesionAsync(string email, string password, bool recordarme) => Task.FromResult(true);

    public Task CerrarSesionAsync() => Task.CompletedTask;

    public Task<Result> RegistrarAsync(string nombreCompleto, string email, string password) => Task.FromResult(Result.Ok());

    public Task<string?> GenerarTokenRecuperacionAsync(string email) =>
        Task.FromResult(TokensPorEmail.TryGetValue(email, out var token) ? token : null);

    public Task<Result> RestablecerPasswordAsync(string email, string token, string nuevaPassword) =>
        Task.FromResult(Result.Ok());
}

internal sealed class FakeNotificacionService : INotificacionService
{
    public List<(string Email, string Enlace)> Enviados { get; } = [];

    public Task EnviarEnlaceRecuperacionPasswordAsync(string email, string enlace, CancellationToken cancellationToken = default)
    {
        Enviados.Add((email, enlace));
        return Task.CompletedTask;
    }
}
