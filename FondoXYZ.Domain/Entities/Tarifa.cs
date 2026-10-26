using FondoXYZ.Domain.Common;

namespace FondoXYZ.Domain.Entities;

public class Tarifa
{
    public int IdTarifa { get; private set; }

    public int IdAlojamiento { get; private set; }

    public int IdTemporada { get; private set; }

    public decimal PrecioBase { get; private set; }

    public decimal PrecioPersonaAdicional { get; private set; }

    public bool Activa { get; private set; }

    public Alojamiento? Alojamiento { get; private set; }

    public Temporada? Temporada { get; private set; }

    // Requerido por EF Core.
    private Tarifa()
    {
    }

    public static Tarifa Crear(
        int idAlojamiento,
        int idTemporada,
        decimal precioBase,
        decimal precioPersonaAdicional,
        bool activa)
    {
        var tarifa = new Tarifa();

        tarifa.Actualizar(idAlojamiento, idTemporada, precioBase, precioPersonaAdicional, activa);

        return tarifa;
    }

    public void Actualizar(
        int idAlojamiento,
        int idTemporada,
        decimal precioBase,
        decimal precioPersonaAdicional,
        bool activa)
    {
        IdAlojamiento = Guard.MayorQueCero(idAlojamiento, "Alojamiento");
        IdTemporada = Guard.MayorQueCero(idTemporada, "Temporada");
        PrecioBase = Guard.MayorQueCero(precioBase, "Precio base");
        PrecioPersonaAdicional = Guard.NoNegativo(precioPersonaAdicional, "Precio persona adicional");
        Activa = activa;
    }
}
