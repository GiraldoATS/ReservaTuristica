using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Services;
using FondoXYZ.Domain.Entities;
using FondoXYZ.Domain.Exceptions;
using FondoXYZ.Domain.ValueObjects;

namespace FondoXYZ.Tests.Application;

public class AlojamientoServiceTests
{
    private const int IdSede = 1;
    private const int IdAlojamiento = 10;

    private readonly FakeAlojamientoRepository _alojamientos = new();
    private readonly FakeSedeRepository _sedes = new();
    private readonly FakeReservaRepository _reservas = new();
    private readonly FakeTarifaRepository _tarifas = new();
    private readonly FakeUnitOfWork _unitOfWork = new();
    private readonly AlojamientoService _sut;

    public AlojamientoServiceTests()
    {
        _sedes.Agregar(Ids.Asignar(Sede.Crear("Sede principal"), nameof(Sede.IdSede), IdSede));

        _sut = new AlojamientoService(_alojamientos, _sedes, _reservas, _tarifas, _unitOfWork);
    }

    private Alojamiento AgregarAlojamiento()
    {
        var alojamiento = Alojamiento.Crear(IdSede, "Cabaña", "Cabaña", 4, 2, null);
        _alojamientos.Agregar(Ids.Asignar(alojamiento, nameof(Alojamiento.IdAlojamiento), IdAlojamiento));
        return alojamiento;
    }

    [Fact]
    public async Task CrearAsync_ConSedeExistente_GuardaAlojamientoActivo()
    {
        await _sut.CrearAsync(new CrearAlojamientoCommand(IdSede, "Cabaña", "Cabaña", 4, 2, null));

        var alojamiento = Assert.Single(_alojamientos.Alojamientos);
        Assert.True(alojamiento.Activo);
        Assert.Equal(1, _unitOfWork.VecesGuardado);
    }

    [Fact]
    public async Task CrearAsync_ConSedeInexistente_LanzaEntidadNoEncontrada()
    {
        await Assert.ThrowsAsync<EntidadNoEncontradaException>(
            () => _sut.CrearAsync(new CrearAlojamientoCommand(99, "Cabaña", "Cabaña", 4, 2, null)));

        Assert.Empty(_alojamientos.Alojamientos);
    }

    [Fact]
    public async Task ActualizarAsync_ModificaLaEntidadExistente()
    {
        var alojamiento = AgregarAlojamiento();

        await _sut.ActualizarAsync(
            new ActualizarAlojamientoCommand(IdAlojamiento, IdSede, "Suite", "Suite", 2, 1, "Vista al mar", Activo: false));

        Assert.Equal("Suite", alojamiento.Nombre);
        Assert.False(alojamiento.Activo);
        Assert.Equal(1, _unitOfWork.VecesGuardado);
    }

    [Fact]
    public async Task EliminarAsync_ConReservasAsociadas_LanzaDomainExceptionYNoElimina()
    {
        var alojamiento = AgregarAlojamiento();
        var periodo = new RangoFechas(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
        _reservas.Agregar(Reserva.Crear("usuario-1", alojamiento, periodo, 2, 100m, DateTime.Now));

        await Assert.ThrowsAsync<DomainException>(() => _sut.EliminarAsync(IdAlojamiento));

        Assert.Single(_alojamientos.Alojamientos);
        Assert.Equal(0, _unitOfWork.VecesGuardado);
    }

    [Fact]
    public async Task EliminarAsync_SinDependencias_Elimina()
    {
        AgregarAlojamiento();

        await _sut.EliminarAsync(IdAlojamiento);

        Assert.Empty(_alojamientos.Alojamientos);
        Assert.Equal(1, _unitOfWork.VecesGuardado);
    }
}
