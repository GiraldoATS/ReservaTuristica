using FondoXYZ.Application.Common;
using FondoXYZ.Application.DTOs;

namespace FondoXYZ.Application.Ports.In;

// Puertos de entrada: lo que la aplicación ofrece a sus adaptadores
// (hoy MVC; mañana podría ser una API REST o un worker) sin exponer el dominio.

public interface ISedeService
{
    Task<IReadOnlyList<SedeDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default);

    Task<int> CrearAsync(CrearSedeCommand command, CancellationToken cancellationToken = default);
}

public interface IAlojamientoService
{
    Task<IReadOnlyList<AlojamientoDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<AlojamientoDto?> ObtenerPorIdAsync(int idAlojamiento, CancellationToken cancellationToken = default);

    Task<int> CrearAsync(CrearAlojamientoCommand command, CancellationToken cancellationToken = default);

    Task ActualizarAsync(ActualizarAlojamientoCommand command, CancellationToken cancellationToken = default);

    Task EliminarAsync(int idAlojamiento, CancellationToken cancellationToken = default);
}

public interface ITemporadaService
{
    Task<IReadOnlyList<TemporadaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default);
}

public interface ITarifaService
{
    Task<IReadOnlyList<TarifaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default);

    Task<TarifaDto?> ObtenerPorIdAsync(int idTarifa, CancellationToken cancellationToken = default);

    Task<int> CrearAsync(CrearTarifaCommand command, CancellationToken cancellationToken = default);

    Task ActualizarAsync(ActualizarTarifaCommand command, CancellationToken cancellationToken = default);

    Task EliminarAsync(int idTarifa, CancellationToken cancellationToken = default);
}

public interface IReservaService
{
    Task<IReadOnlyList<ReservaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default);

    Task<int> CrearAsync(CrearReservaCommand command, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DisponibilidadDto>> ConsultarDisponibilidadAsync(
        DateTime fechaInicio,
        DateTime fechaFin,
        CancellationToken cancellationToken = default);

    Task<CalculoTarifaDto> CalcularTarifaAsync(
        int idAlojamiento,
        int numeroPersonas,
        DateTime fechaInicio,
        DateTime fechaFin,
        CancellationToken cancellationToken = default);
}

public interface IDashboardService
{
    Task<DashboardDto> ObtenerAsync(CancellationToken cancellationToken = default);
}

public interface IAuthService
{
    Task<bool> LoginAsync(LoginCommand command);

    Task<Result> RegistrarAsync(RegistrarUsuarioCommand command);

    Task LogoutAsync();

    /// <param name="urlRestablecimiento">URL absoluta de la página de restablecimiento; la provee el adaptador web.</param>
    Task SolicitarRecuperacionPasswordAsync(
        string email,
        string urlRestablecimiento,
        CancellationToken cancellationToken = default);

    Task<Result> RestablecerPasswordAsync(RestablecerPasswordCommand command);
}
