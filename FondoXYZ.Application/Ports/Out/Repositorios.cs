using FondoXYZ.Domain.Entities;
using FondoXYZ.Domain.ValueObjects;

namespace FondoXYZ.Application.Ports.Out;

// Puertos de salida de persistencia. Los repositorios solo registran
// cambios; la confirmación la hace IUnitOfWork para que un caso de uso
// pueda guardar varias operaciones en una sola transacción.

public interface IUnitOfWork
{
    Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default);
}

public interface ISedeRepository
{
    Task<IReadOnlyList<Sede>> ObtenerTodasAsync(CancellationToken cancellationToken = default);

    Task<bool> ExisteAsync(int idSede, CancellationToken cancellationToken = default);

    void Agregar(Sede sede);
}

public interface IAlojamientoRepository
{
    Task<IReadOnlyList<Alojamiento>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<Alojamiento?> ObtenerPorIdAsync(int idAlojamiento, CancellationToken cancellationToken = default);

    Task<bool> ExisteAsync(int idAlojamiento, CancellationToken cancellationToken = default);

    void Agregar(Alojamiento alojamiento);

    void Eliminar(Alojamiento alojamiento);
}

public interface ITemporadaRepository
{
    Task<IReadOnlyList<Temporada>> ObtenerTodasAsync(CancellationToken cancellationToken = default);

    Task<bool> ExisteAsync(int idTemporada, CancellationToken cancellationToken = default);
}

public interface ITarifaRepository
{
    Task<IReadOnlyList<Tarifa>> ObtenerTodasAsync(CancellationToken cancellationToken = default);

    Task<Tarifa?> ObtenerPorIdAsync(int idTarifa, CancellationToken cancellationToken = default);

    Task<bool> ExisteParaAlojamientoAsync(int idAlojamiento, CancellationToken cancellationToken = default);

    void Agregar(Tarifa tarifa);

    void Eliminar(Tarifa tarifa);
}

public interface IReservaRepository
{
    Task<IReadOnlyList<Reserva>> ObtenerTodasAsync(CancellationToken cancellationToken = default);

    Task<bool> ExisteCruceAsync(int idAlojamiento, RangoFechas periodo, CancellationToken cancellationToken = default);

    Task<bool> ExisteParaAlojamientoAsync(int idAlojamiento, CancellationToken cancellationToken = default);

    void Agregar(Reserva reserva);
}
