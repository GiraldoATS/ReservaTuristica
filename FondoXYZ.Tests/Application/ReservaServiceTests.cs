using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Services;
using FondoXYZ.Domain.Entities;
using FondoXYZ.Domain.Exceptions;
using FondoXYZ.Domain.ValueObjects;

namespace FondoXYZ.Tests.Application;

public class ReservaServiceTests
{
    private const int IdAlojamiento = 7;
    private static readonly DateTime Hoy = new(2026, 10, 26, 10, 0, 0);

    private readonly FakeReservaRepository _reservas = new();
    private readonly FakeAlojamientoRepository _alojamientos = new();
    private readonly FakeDisponibilidadQuery _disponibilidad = new();
    private readonly FakeCalculoTarifaQuery _calculoTarifa = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly ReservaService _sut;

    public ReservaServiceTests()
    {
        var alojamiento = Alojamiento.Crear(idSede: 1, "Cabaña", "Cabaña", capacidadMaxima: 4, cantidadHabitaciones: 2, null);
        _alojamientos.Agregar(Ids.Asignar(alojamiento, nameof(Alojamiento.IdAlojamiento), IdAlojamiento));

        _sut = new ReservaService(
            _reservas,
            _alojamientos,
            _disponibilidad,
            _calculoTarifa,
            _unitOfWork,
            new FakeTimeProvider(Hoy));
    }

    private static CrearReservaCommand Comando(int diasDesdeHoy = 1, int noches = 2, int personas = 2) =>
        new("usuario-1", IdAlojamiento, Hoy.Date.AddDays(diasDesdeHoy), Hoy.Date.AddDays(diasDesdeHoy + noches), personas);

    [Fact]
    public async Task CrearAsync_ConDatosValidos_GuardaReservaConTotalCalculadoEnServidor()
    {
        _calculoTarifa.PrecioPorNoche = 150_000m;

        await _sut.CrearAsync(Comando(noches: 3));

        var reserva = Assert.Single(_reservas.Reservas);
        Assert.Equal(450_000m, reserva.Total);
        Assert.Equal(IdAlojamiento, reserva.IdAlojamiento);
        Assert.Equal(1, _unitOfWork.VecesGuardado);
    }

    [Fact]
    public async Task CrearAsync_ConCruceDeFechas_LanzaDomainExceptionYNoGuarda()
    {
        await _sut.CrearAsync(Comando(diasDesdeHoy: 1, noches: 3));

        var ex = await Assert.ThrowsAsync<DomainException>(() => _sut.CrearAsync(Comando(diasDesdeHoy: 2, noches: 3)));

        Assert.Contains("Ya existe una reserva", ex.Message);
        Assert.Single(_reservas.Reservas);
        Assert.Equal(1, _unitOfWork.VecesGuardado);
    }

    [Fact]
    public async Task CrearAsync_ReservaQueIniciaElDiaDelCheckoutAnterior_EsValida()
    {
        await _sut.CrearAsync(Comando(diasDesdeHoy: 1, noches: 2));
        await _sut.CrearAsync(Comando(diasDesdeHoy: 3, noches: 2));

        Assert.Equal(2, _reservas.Reservas.Count);
    }

    [Fact]
    public async Task CrearAsync_SinTarifaVigente_LanzaDomainException()
    {
        _calculoTarifa.PrecioPorNoche = null;

        var ex = await Assert.ThrowsAsync<DomainException>(() => _sut.CrearAsync(Comando()));

        Assert.Contains("tarifa vigente", ex.Message);
        Assert.Empty(_reservas.Reservas);
    }

    [Fact]
    public async Task CrearAsync_ConAlojamientoInexistente_LanzaEntidadNoEncontrada()
    {
        var comando = Comando() with { IdAlojamiento = 999 };

        await Assert.ThrowsAsync<EntidadNoEncontradaException>(() => _sut.CrearAsync(comando));
    }

    [Fact]
    public async Task CrearAsync_SuperandoCapacidad_LanzaDomainException()
    {
        await Assert.ThrowsAsync<DomainException>(() => _sut.CrearAsync(Comando(personas: 5)));

        Assert.Empty(_reservas.Reservas);
    }

    [Fact]
    public async Task ConsultarDisponibilidadAsync_ConFechaPasada_LanzaDomainException()
    {
        await Assert.ThrowsAsync<DomainException>(
            () => _sut.ConsultarDisponibilidadAsync(Hoy.AddDays(-1), Hoy.AddDays(2)));

        Assert.Null(_disponibilidad.UltimoPeriodoConsultado);
    }

    [Fact]
    public async Task ConsultarDisponibilidadAsync_ConFechasValidas_ConsultaElPuerto()
    {
        await _sut.ConsultarDisponibilidadAsync(Hoy, Hoy.AddDays(2));

        Assert.Equal(new RangoFechas(Hoy, Hoy.AddDays(2)), _disponibilidad.UltimoPeriodoConsultado);
    }

    [Fact]
    public async Task CalcularTarifaAsync_SinPersonas_LanzaDomainException()
    {
        await Assert.ThrowsAsync<DomainException>(
            () => _sut.CalcularTarifaAsync(IdAlojamiento, 0, Hoy, Hoy.AddDays(1)));
    }
}
