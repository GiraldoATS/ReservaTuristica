namespace FondoXYZ.Domain.Entities;

public class Temporada
{
    public const int LongitudMaximaNombre = 100;

    public int IdTemporada { get; private set; }

    public string Nombre { get; private set; } = string.Empty;

    public DateTime FechaInicio { get; private set; }

    public DateTime FechaFin { get; private set; }

    public bool Activa { get; private set; }

    // Requerido por EF Core.
    private Temporada()
    {
    }
}
