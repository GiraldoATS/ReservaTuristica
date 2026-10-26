using FondoXYZ.Domain.Exceptions;
using FondoXYZ.Domain.ValueObjects;

namespace FondoXYZ.Tests.Domain;

public class RangoFechasTests
{
    [Fact]
    public void Crear_ConFechaFinPosterior_CalculaNoches()
    {
        var rango = new RangoFechas(new DateTime(2026, 12, 1), new DateTime(2026, 12, 4));

        Assert.Equal(3, rango.Noches);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Crear_ConFechaFinIgualOAnterior_LanzaDomainException(int diasDiferencia)
    {
        var inicio = new DateTime(2026, 12, 1);

        Assert.Throws<DomainException>(() => new RangoFechas(inicio, inicio.AddDays(diasDiferencia)));
    }

    [Fact]
    public void Crear_IgnoraLaHora()
    {
        var rango = new RangoFechas(new DateTime(2026, 12, 1, 15, 30, 0), new DateTime(2026, 12, 2, 8, 0, 0));

        Assert.Equal(new DateTime(2026, 12, 1), rango.FechaInicio);
        Assert.Equal(1, rango.Noches);
    }

    [Theory]
    [InlineData(1, 5, 4, 8, true)]   // se solapan
    [InlineData(1, 5, 2, 3, true)]   // uno contiene al otro
    [InlineData(1, 5, 5, 8, false)]  // el checkout coincide con el siguiente check-in
    [InlineData(1, 5, 6, 8, false)]  // separados
    public void SeCruzaCon_DetectaSolapamientos(int inicioA, int finA, int inicioB, int finB, bool esperado)
    {
        var a = new RangoFechas(new DateTime(2026, 12, inicioA), new DateTime(2026, 12, finA));
        var b = new RangoFechas(new DateTime(2026, 12, inicioB), new DateTime(2026, 12, finB));

        Assert.Equal(esperado, a.SeCruzaCon(b));
        Assert.Equal(esperado, b.SeCruzaCon(a));
    }

    [Fact]
    public void ValidarQueNoIniciaEnElPasado_ConFechaAnteriorAHoy_LanzaDomainException()
    {
        var rango = new RangoFechas(new DateTime(2026, 10, 25), new DateTime(2026, 10, 27));

        Assert.Throws<DomainException>(() => rango.ValidarQueNoIniciaEnElPasado(new DateTime(2026, 10, 26, 9, 0, 0)));
    }
}
