using FondoXYZ.Domain.Common;

namespace FondoXYZ.Domain.Entities;

public class Sede
{
    public const int LongitudMaximaNombre = 150;

    public int IdSede { get; private set; }

    public string Nombre { get; private set; } = string.Empty;

    public string Ciudad { get; private set; } = string.Empty;

    public string Tipo { get; private set; } = string.Empty;

    public string Descripcion { get; private set; } = string.Empty;

    public int CapacidadTotal { get; private set; }

    public bool Activa { get; private set; }

    // Requerido por EF Core.
    private Sede()
    {
    }

    public static Sede Crear(
        string nombre,
        string? ciudad = null,
        string? tipo = null,
        string? descripcion = null,
        int capacidadTotal = 0)
    {
        return new Sede
        {
            Nombre = Guard.Requerido(nombre, "Nombre", LongitudMaximaNombre),
            Ciudad = ciudad?.Trim() ?? string.Empty,
            Tipo = tipo?.Trim() ?? string.Empty,
            Descripcion = descripcion?.Trim() ?? string.Empty,
            CapacidadTotal = Guard.NoNegativo(capacidadTotal, "Capacidad total"),
            Activa = true
        };
    }
}
