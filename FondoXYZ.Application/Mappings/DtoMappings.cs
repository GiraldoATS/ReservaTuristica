using FondoXYZ.Application.DTOs;
using FondoXYZ.Domain.Entities;

namespace FondoXYZ.Application.Mappings;

/// <summary>
/// Traducción de entidades de dominio a DTOs. Así el dominio nunca
/// sale de la capa de aplicación hacia los adaptadores de entrada.
/// </summary>
public static class DtoMappings
{
    public static SedeDto ToDto(this Sede sede) => new(
        sede.IdSede,
        sede.Nombre,
        sede.Ciudad,
        sede.Tipo,
        sede.Descripcion,
        sede.CapacidadTotal,
        sede.Activa);

    public static AlojamientoDto ToDto(this Alojamiento alojamiento) => new(
        alojamiento.IdAlojamiento,
        alojamiento.IdSede,
        alojamiento.Sede?.Nombre ?? string.Empty,
        alojamiento.Nombre,
        alojamiento.Tipo,
        alojamiento.CapacidadMaxima,
        alojamiento.CantidadHabitaciones,
        alojamiento.Descripcion,
        alojamiento.Activo);

    public static TemporadaDto ToDto(this Temporada temporada) => new(
        temporada.IdTemporada,
        temporada.Nombre,
        temporada.FechaInicio,
        temporada.FechaFin,
        temporada.Activa);

    public static TarifaDto ToDto(this Tarifa tarifa) => new(
        tarifa.IdTarifa,
        tarifa.IdAlojamiento,
        tarifa.Alojamiento?.Nombre ?? string.Empty,
        tarifa.IdTemporada,
        tarifa.Temporada?.Nombre ?? string.Empty,
        tarifa.PrecioBase,
        tarifa.PrecioPersonaAdicional,
        tarifa.Activa);

    public static ReservaDto ToDto(this Reserva reserva) => new(
        reserva.IdReserva,
        reserva.IdAlojamiento,
        reserva.Alojamiento?.Nombre ?? string.Empty,
        reserva.FechaInicio,
        reserva.FechaFin,
        reserva.NumeroPersonas,
        reserva.Total,
        reserva.Estado.ToString().ToUpperInvariant(),
        reserva.FechaReserva);
}
