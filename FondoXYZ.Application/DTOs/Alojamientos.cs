namespace FondoXYZ.Application.DTOs;

public sealed record AlojamientoDto(
    int IdAlojamiento,
    int IdSede,
    string Sede,
    string Nombre,
    string Tipo,
    int CapacidadMaxima,
    int CantidadHabitaciones,
    string Descripcion,
    bool Activo);

public sealed record CrearAlojamientoCommand(
    int IdSede,
    string Nombre,
    string Tipo,
    int CapacidadMaxima,
    int CantidadHabitaciones,
    string? Descripcion);

public sealed record ActualizarAlojamientoCommand(
    int IdAlojamiento,
    int IdSede,
    string Nombre,
    string Tipo,
    int CapacidadMaxima,
    int CantidadHabitaciones,
    string? Descripcion,
    bool Activo);
