using FondoXYZ.Domain.Entities;
using FondoXYZ.Domain.Exceptions;

namespace FondoXYZ.Tests.Domain;

public class EntidadesTests
{
    [Fact]
    public void Alojamiento_Crear_QuedaActivoYNormalizaTextos()
    {
        var alojamiento = Alojamiento.Crear(1, "  Cabaña del lago ", "Cabaña", 4, 2, null);

        Assert.True(alojamiento.Activo);
        Assert.Equal("Cabaña del lago", alojamiento.Nombre);
        Assert.Equal(string.Empty, alojamiento.Descripcion);
    }

    [Theory]
    [InlineData("", 4)]
    [InlineData("Cabaña", 0)]
    public void Alojamiento_Crear_ConDatosInvalidos_LanzaDomainException(string nombre, int capacidad)
    {
        Assert.Throws<DomainException>(() => Alojamiento.Crear(1, nombre, "Cabaña", capacidad, 1, null));
    }

    [Fact]
    public void Sede_Crear_QuedaActiva()
    {
        var sede = Sede.Crear("Sede Rionegro");

        Assert.True(sede.Activa);
        Assert.Equal("Sede Rionegro", sede.Nombre);
    }

    [Fact]
    public void Sede_Crear_ConNombreDemasiadoLargo_LanzaDomainException()
    {
        Assert.Throws<DomainException>(() => Sede.Crear(new string('x', Sede.LongitudMaximaNombre + 1)));
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(100, -1)]
    public void Tarifa_Crear_ConPreciosInvalidos_LanzaDomainException(decimal precioBase, decimal precioAdicional)
    {
        Assert.Throws<DomainException>(() => Tarifa.Crear(1, 1, precioBase, precioAdicional, activa: true));
    }
}
