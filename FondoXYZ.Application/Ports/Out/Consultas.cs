using FondoXYZ.Application.DTOs;
using FondoXYZ.Domain.ValueObjects;

namespace FondoXYZ.Application.Ports.Out;

// Puertos de solo lectura. Se separan de los repositorios (ISP) porque
// devuelven proyecciones y hoy se resuelven con procedimientos almacenados.

public interface IDisponibilidadQuery
{
    Task<IReadOnlyList<DisponibilidadDto>> ConsultarAsync(RangoFechas periodo, CancellationToken cancellationToken = default);
}

public interface ICalculoTarifaQuery
{
    /// <returns>El cálculo, o null si no hay temporada/tarifa vigente para el periodo.</returns>
    Task<CalculoTarifaDto?> CalcularAsync(
        int idAlojamiento,
        int numeroPersonas,
        RangoFechas periodo,
        CancellationToken cancellationToken = default);
}

public interface IDashboardQuery
{
    Task<DashboardDto> ObtenerAsync(CancellationToken cancellationToken = default);
}
