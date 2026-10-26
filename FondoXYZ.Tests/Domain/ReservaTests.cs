using FondoXYZ.Domain.Entities;
using FondoXYZ.Domain.Enums;
using FondoXYZ.Domain.Exceptions;
using FondoXYZ.Domain.ValueObjects;

namespace FondoXYZ.Tests.Domain;

public class ReservaTests
{
    private static readonly DateTime Hoy = new(2026, 10, 26, 10, 0, 0);

    private static readonly RangoFechas PeriodoValido = new(Hoy.AddDays(1), Hoy.AddDays(3));

    private static Alojamiento CrearAlojamiento(int capacidad = 4) =>
        Alojamiento.Crear(idSede: 1, "Cabaña", "Cabaña", capacidad, cantidadHabitaciones: 2, descripcion: null);

    [Fact]
    public void Crear_ConDatosValidos_QuedaActiva()
    {
        var reserva = Reserva.Crear("usuario-1", CrearAlojamiento(), PeriodoValido, 2, 500_000m, Hoy);

        Assert.Equal(EstadoReserva.Activa, reserva.Estado);
        Assert.Equal(500_000m, reserva.Total);
        Assert.Equal(PeriodoValido, reserva.Periodo);
    }

    [Fact]
    public void Crear_SuperandoCapacidad_LanzaDomainException()
    {
        var ex = Assert.Throws<DomainException>(
            () => Reserva.Crear("usuario-1", CrearAlojamiento(capacidad: 2), PeriodoValido, 3, 100m, Hoy));

        Assert.Contains("capacidad máxima", ex.Message);
    }

    [Fact]
    public void Crear_ConFechaInicioEnElPasado_LanzaDomainException()
    {
        var periodoPasado = new RangoFechas(Hoy.AddDays(-2), Hoy.AddDays(1));

        Assert.Throws<DomainException>(
            () => Reserva.Crear("usuario-1", CrearAlojamiento(), periodoPasado, 2, 100m, Hoy));
    }

    [Fact]
    public void Crear_EnAlojamientoInactivo_LanzaDomainException()
    {
        var alojamiento = CrearAlojamiento();
        alojamiento.Actualizar(1, "Cabaña", "Cabaña", 4, 2, null, activo: false);

        Assert.Throws<DomainException>(
            () => Reserva.Crear("usuario-1", alojamiento, PeriodoValido, 2, 100m, Hoy));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Crear_ConTotalNoPositivo_LanzaDomainException(decimal total)
    {
        Assert.Throws<DomainException>(
            () => Reserva.Crear("usuario-1", CrearAlojamiento(), PeriodoValido, 2, total, Hoy));
    }

    [Fact]
    public void Cancelar_DosVeces_LanzaDomainException()
    {
        var reserva = Reserva.Crear("usuario-1", CrearAlojamiento(), PeriodoValido, 2, 100m, Hoy);

        reserva.Cancelar();

        Assert.Equal(EstadoReserva.Cancelada, reserva.Estado);
        Assert.Throws<DomainException>(reserva.Cancelar);
    }
}
