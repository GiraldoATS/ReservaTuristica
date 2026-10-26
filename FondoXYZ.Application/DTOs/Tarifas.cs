namespace FondoXYZ.Application.DTOs;

public sealed record TemporadaDto(
    int IdTemporada,
    string Nombre,
    DateTime FechaInicio,
    DateTime FechaFin,
    bool Activa);

public sealed record TarifaDto(
    int IdTarifa,
    int IdAlojamiento,
    string Alojamiento,
    int IdTemporada,
    string Temporada,
    decimal PrecioBase,
    decimal PrecioPersonaAdicional,
    bool Activa);

public sealed record CrearTarifaCommand(
    int IdAlojamiento,
    int IdTemporada,
    decimal PrecioBase,
    decimal PrecioPersonaAdicional,
    bool Activa);

public sealed record ActualizarTarifaCommand(
    int IdTarifa,
    int IdAlojamiento,
    int IdTemporada,
    decimal PrecioBase,
    decimal PrecioPersonaAdicional,
    bool Activa);
