namespace FondoXYZ.Application.DTOs;

public sealed record SedeDto(
    int IdSede,
    string Nombre,
    string Ciudad,
    string Tipo,
    string Descripcion,
    int CapacidadTotal,
    bool Activa);

public sealed record CrearSedeCommand(
    string Nombre,
    string? Ciudad = null,
    string? Tipo = null,
    string? Descripcion = null,
    int CapacidadTotal = 0);
