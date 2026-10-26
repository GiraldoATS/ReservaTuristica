using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Mappings;
using FondoXYZ.Application.Ports.In;
using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.Entities;
using FondoXYZ.Domain.Exceptions;

namespace FondoXYZ.Application.Services;

internal sealed class AlojamientoService : IAlojamientoService
{
    private readonly IAlojamientoRepository _alojamientos;
    private readonly ISedeRepository _sedes;
    private readonly IReservaRepository _reservas;
    private readonly ITarifaRepository _tarifas;
    private readonly IUnitOfWork _unitOfWork;

    public AlojamientoService(
        IAlojamientoRepository alojamientos,
        ISedeRepository sedes,
        IReservaRepository reservas,
        ITarifaRepository tarifas,
        IUnitOfWork unitOfWork)
    {
        _alojamientos = alojamientos;
        _sedes = sedes;
        _reservas = reservas;
        _tarifas = tarifas;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<AlojamientoDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var alojamientos = await _alojamientos.ObtenerTodosAsync(cancellationToken);

        return alojamientos.Select(a => a.ToDto()).ToList();
    }

    public async Task<AlojamientoDto?> ObtenerPorIdAsync(int idAlojamiento, CancellationToken cancellationToken = default)
    {
        var alojamiento = await _alojamientos.ObtenerPorIdAsync(idAlojamiento, cancellationToken);

        return alojamiento?.ToDto();
    }

    public async Task<int> CrearAsync(CrearAlojamientoCommand command, CancellationToken cancellationToken = default)
    {
        await ValidarSedeExisteAsync(command.IdSede, cancellationToken);

        var alojamiento = Alojamiento.Crear(
            command.IdSede,
            command.Nombre,
            command.Tipo,
            command.CapacidadMaxima,
            command.CantidadHabitaciones,
            command.Descripcion);

        _alojamientos.Agregar(alojamiento);
        await _unitOfWork.GuardarCambiosAsync(cancellationToken);

        return alojamiento.IdAlojamiento;
    }

    public async Task ActualizarAsync(ActualizarAlojamientoCommand command, CancellationToken cancellationToken = default)
    {
        var alojamiento = await ObtenerExistenteAsync(command.IdAlojamiento, cancellationToken);

        if (alojamiento.IdSede != command.IdSede)
            await ValidarSedeExisteAsync(command.IdSede, cancellationToken);

        alojamiento.Actualizar(
            command.IdSede,
            command.Nombre,
            command.Tipo,
            command.CapacidadMaxima,
            command.CantidadHabitaciones,
            command.Descripcion,
            command.Activo);

        await _unitOfWork.GuardarCambiosAsync(cancellationToken);
    }

    public async Task EliminarAsync(int idAlojamiento, CancellationToken cancellationToken = default)
    {
        var alojamiento = await ObtenerExistenteAsync(idAlojamiento, cancellationToken);

        if (await _reservas.ExisteParaAlojamientoAsync(idAlojamiento, cancellationToken))
            throw new DomainException("No se puede eliminar el alojamiento porque tiene reservas asociadas. Puede desactivarlo.");

        if (await _tarifas.ExisteParaAlojamientoAsync(idAlojamiento, cancellationToken))
            throw new DomainException("No se puede eliminar el alojamiento porque tiene tarifas asociadas.");

        _alojamientos.Eliminar(alojamiento);
        await _unitOfWork.GuardarCambiosAsync(cancellationToken);
    }

    private async Task<Alojamiento> ObtenerExistenteAsync(int idAlojamiento, CancellationToken cancellationToken)
    {
        return await _alojamientos.ObtenerPorIdAsync(idAlojamiento, cancellationToken)
            ?? throw new EntidadNoEncontradaException("El alojamiento", idAlojamiento);
    }

    private async Task ValidarSedeExisteAsync(int idSede, CancellationToken cancellationToken)
    {
        if (!await _sedes.ExisteAsync(idSede, cancellationToken))
            throw new EntidadNoEncontradaException("La sede", idSede);
    }
}
