using FondoXYZ.Domain.Common;
using FondoXYZ.Domain.Exceptions;

namespace FondoXYZ.Domain.Entities;

public class Alojamiento
{
    public const int LongitudMaximaNombre = 150;

    public int IdAlojamiento { get; private set; }

    public int IdSede { get; private set; }

    public string Nombre { get; private set; } = string.Empty;

    public string Tipo { get; private set; } = string.Empty;

    public int CapacidadMaxima { get; private set; }

    public int CantidadHabitaciones { get; private set; }

    public string Descripcion { get; private set; } = string.Empty;

    public bool Activo { get; private set; }

    public Sede? Sede { get; private set; }

    // Requerido por EF Core.
    private Alojamiento()
    {
    }

    public static Alojamiento Crear(
        int idSede,
        string nombre,
        string tipo,
        int capacidadMaxima,
        int cantidadHabitaciones,
        string? descripcion)
    {
        var alojamiento = new Alojamiento { Activo = true };

        alojamiento.EstablecerDatos(idSede, nombre, tipo, capacidadMaxima, cantidadHabitaciones, descripcion);

        return alojamiento;
    }

    public void Actualizar(
        int idSede,
        string nombre,
        string tipo,
        int capacidadMaxima,
        int cantidadHabitaciones,
        string? descripcion,
        bool activo)
    {
        EstablecerDatos(idSede, nombre, tipo, capacidadMaxima, cantidadHabitaciones, descripcion);
        Activo = activo;
    }

    public void ValidarDisponibleParaReserva(int numeroPersonas)
    {
        if (!Activo)
            throw new DomainException("El alojamiento no está disponible para reservas.");

        if (numeroPersonas <= 0)
            throw new DomainException("Debe ingresar mínimo 1 persona.");

        if (numeroPersonas > CapacidadMaxima)
            throw new DomainException($"La capacidad máxima es de {CapacidadMaxima} personas.");
    }

    private void EstablecerDatos(
        int idSede,
        string nombre,
        string tipo,
        int capacidadMaxima,
        int cantidadHabitaciones,
        string? descripcion)
    {
        IdSede = Guard.MayorQueCero(idSede, "Sede");
        Nombre = Guard.Requerido(nombre, "Nombre", LongitudMaximaNombre);
        Tipo = Guard.Requerido(tipo, "Tipo");
        CapacidadMaxima = Guard.MayorQueCero(capacidadMaxima, "Capacidad máxima");
        CantidadHabitaciones = Guard.NoNegativo(cantidadHabitaciones, "Cantidad de habitaciones");
        Descripcion = descripcion?.Trim() ?? string.Empty;
    }
}
