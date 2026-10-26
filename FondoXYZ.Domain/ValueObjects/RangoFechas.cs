using FondoXYZ.Domain.Exceptions;

namespace FondoXYZ.Domain.ValueObjects;

/// <summary>
/// Periodo de estadía. Es inmutable y siempre válido: la fecha fin
/// es posterior a la fecha inicio, por lo que hay al menos una noche.
/// </summary>
public sealed record RangoFechas
{
    public DateTime FechaInicio { get; }

    public DateTime FechaFin { get; }

    public RangoFechas(DateTime fechaInicio, DateTime fechaFin)
    {
        if (fechaFin.Date <= fechaInicio.Date)
            throw new DomainException("La fecha fin debe ser mayor a la fecha inicio.");

        FechaInicio = fechaInicio.Date;
        FechaFin = fechaFin.Date;
    }

    public int Noches => (FechaFin - FechaInicio).Days;

    public bool SeCruzaCon(RangoFechas otro) =>
        FechaInicio < otro.FechaFin && otro.FechaInicio < FechaFin;

    public void ValidarQueNoIniciaEnElPasado(DateTime fechaActual)
    {
        if (FechaInicio < fechaActual.Date)
            throw new DomainException("La fecha inicio no puede ser menor a hoy.");
    }
}
