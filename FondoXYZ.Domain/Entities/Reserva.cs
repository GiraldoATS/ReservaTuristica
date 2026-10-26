using FondoXYZ.Domain.Common;
using FondoXYZ.Domain.Enums;
using FondoXYZ.Domain.Exceptions;
using FondoXYZ.Domain.ValueObjects;

namespace FondoXYZ.Domain.Entities;

public class Reserva
{
    public int IdReserva { get; private set; }

    public string IdUsuario { get; private set; } = string.Empty;

    public int IdAlojamiento { get; private set; }

    public DateTime FechaInicio { get; private set; }

    public DateTime FechaFin { get; private set; }

    public int NumeroPersonas { get; private set; }

    public decimal Total { get; private set; }

    public EstadoReserva Estado { get; private set; }

    public DateTime FechaReserva { get; private set; }

    public Alojamiento? Alojamiento { get; private set; }

    public RangoFechas Periodo => new(FechaInicio, FechaFin);

    // Requerido por EF Core.
    private Reserva()
    {
    }

    /// <summary>
    /// Crea una reserva garantizando las reglas de negocio:
    /// alojamiento activo, capacidad, fechas futuras y total positivo.
    /// La verificación de cruces con otras reservas requiere consultar
    /// persistencia, por eso la hace el caso de uso antes de llamar aquí.
    /// </summary>
    public static Reserva Crear(
        string idUsuario,
        Alojamiento alojamiento,
        RangoFechas periodo,
        int numeroPersonas,
        decimal total,
        DateTime fechaActual)
    {
        ArgumentNullException.ThrowIfNull(alojamiento);
        ArgumentNullException.ThrowIfNull(periodo);

        periodo.ValidarQueNoIniciaEnElPasado(fechaActual);
        alojamiento.ValidarDisponibleParaReserva(numeroPersonas);

        return new Reserva
        {
            IdUsuario = Guard.Requerido(idUsuario, "Usuario"),
            IdAlojamiento = alojamiento.IdAlojamiento,
            FechaInicio = periodo.FechaInicio,
            FechaFin = periodo.FechaFin,
            NumeroPersonas = numeroPersonas,
            Total = Guard.MayorQueCero(total, "Total"),
            Estado = EstadoReserva.Activa,
            FechaReserva = fechaActual
        };
    }

    public void Cancelar()
    {
        if (Estado == EstadoReserva.Cancelada)
            throw new DomainException("La reserva ya se encuentra cancelada.");

        Estado = EstadoReserva.Cancelada;
    }
}
