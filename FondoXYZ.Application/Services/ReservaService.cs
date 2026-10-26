using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Mappings;
using FondoXYZ.Application.Ports.In;
using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.Entities;
using FondoXYZ.Domain.Exceptions;
using FondoXYZ.Domain.ValueObjects;

namespace FondoXYZ.Application.Services;

internal sealed class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservas;
    private readonly IAlojamientoRepository _alojamientos;
    private readonly IDisponibilidadQuery _disponibilidad;
    private readonly ICalculoTarifaQuery _calculoTarifa;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;

    public ReservaService(
        IReservaRepository reservas,
        IAlojamientoRepository alojamientos,
        IDisponibilidadQuery disponibilidad,
        ICalculoTarifaQuery calculoTarifa,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider)
    {
        _reservas = reservas;
        _alojamientos = alojamientos;
        _disponibilidad = disponibilidad;
        _calculoTarifa = calculoTarifa;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }

    private DateTime Ahora => _timeProvider.GetLocalNow().DateTime;

    public async Task<IReadOnlyList<ReservaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        var reservas = await _reservas.ObtenerTodasAsync(cancellationToken);

        return reservas.Select(r => r.ToDto()).ToList();
    }

    public async Task<int> CrearAsync(CrearReservaCommand command, CancellationToken cancellationToken = default)
    {
        var periodo = new RangoFechas(command.FechaInicio, command.FechaFin);
        periodo.ValidarQueNoIniciaEnElPasado(Ahora);

        var alojamiento = await _alojamientos.ObtenerPorIdAsync(command.IdAlojamiento, cancellationToken)
            ?? throw new EntidadNoEncontradaException("El alojamiento", command.IdAlojamiento);

        alojamiento.ValidarDisponibleParaReserva(command.NumeroPersonas);

        if (await _reservas.ExisteCruceAsync(alojamiento.IdAlojamiento, periodo, cancellationToken))
            throw new DomainException("Ya existe una reserva para este alojamiento en las fechas seleccionadas.");

        // El total siempre se calcula en el servidor; nunca se confía en el valor enviado por el cliente.
        var calculo = await CalcularAsync(alojamiento.IdAlojamiento, command.NumeroPersonas, periodo, cancellationToken);

        var reserva = Reserva.Crear(
            command.IdUsuario,
            alojamiento,
            periodo,
            command.NumeroPersonas,
            calculo.TotalPagar,
            Ahora);

        _reservas.Agregar(reserva);
        await _unitOfWork.GuardarCambiosAsync(cancellationToken);

        return reserva.IdReserva;
    }

    public async Task<IReadOnlyList<DisponibilidadDto>> ConsultarDisponibilidadAsync(
        DateTime fechaInicio,
        DateTime fechaFin,
        CancellationToken cancellationToken = default)
    {
        var periodo = new RangoFechas(fechaInicio, fechaFin);
        periodo.ValidarQueNoIniciaEnElPasado(Ahora);

        return await _disponibilidad.ConsultarAsync(periodo, cancellationToken);
    }

    public Task<CalculoTarifaDto> CalcularTarifaAsync(
        int idAlojamiento,
        int numeroPersonas,
        DateTime fechaInicio,
        DateTime fechaFin,
        CancellationToken cancellationToken = default)
    {
        if (numeroPersonas <= 0)
            throw new DomainException("Debe ingresar mínimo 1 persona.");

        var periodo = new RangoFechas(fechaInicio, fechaFin);

        return CalcularAsync(idAlojamiento, numeroPersonas, periodo, cancellationToken);
    }

    private async Task<CalculoTarifaDto> CalcularAsync(
        int idAlojamiento,
        int numeroPersonas,
        RangoFechas periodo,
        CancellationToken cancellationToken)
    {
        return await _calculoTarifa.CalcularAsync(idAlojamiento, numeroPersonas, periodo, cancellationToken)
            ?? throw new DomainException("No hay una tarifa vigente para este alojamiento en las fechas seleccionadas.");
    }
}
