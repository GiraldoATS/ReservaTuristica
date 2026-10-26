namespace FondoXYZ.Application.DTOs;

public sealed record ReservaDto(
    int IdReserva,
    int IdAlojamiento,
    string Alojamiento,
    DateTime FechaInicio,
    DateTime FechaFin,
    int NumeroPersonas,
    decimal Total,
    string Estado,
    DateTime FechaReserva);

public sealed record CrearReservaCommand(
    string IdUsuario,
    int IdAlojamiento,
    DateTime FechaInicio,
    DateTime FechaFin,
    int NumeroPersonas);

public sealed record DisponibilidadDto(
    int IdAlojamiento,
    string Sede,
    string Alojamiento,
    int CapacidadMaxima,
    string Tipo);

public sealed record CalculoTarifaDto(
    int IdTemporada,
    decimal PrecioBase,
    decimal PrecioPersonaAdicional,
    int NumeroNoches,
    int PersonasAdicionales,
    decimal TotalPagar);

public sealed record DashboardDto(
    int TotalReservas,
    decimal IngresosTotales,
    int TotalAlojamientos,
    int TarifasActivas,
    IReadOnlyList<ReservaDto> UltimasReservas);
