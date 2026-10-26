using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Mappings;
using FondoXYZ.Application.Ports.In;
using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.Entities;
using FondoXYZ.Domain.Exceptions;

namespace FondoXYZ.Application.Services;

internal sealed class TarifaService : ITarifaService
{
    private readonly ITarifaRepository _tarifas;
    private readonly IAlojamientoRepository _alojamientos;
    private readonly ITemporadaRepository _temporadas;
    private readonly IUnitOfWork _unitOfWork;

    public TarifaService(
        ITarifaRepository tarifas,
        IAlojamientoRepository alojamientos,
        ITemporadaRepository temporadas,
        IUnitOfWork unitOfWork)
    {
        _tarifas = tarifas;
        _alojamientos = alojamientos;
        _temporadas = temporadas;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TarifaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        var tarifas = await _tarifas.ObtenerTodasAsync(cancellationToken);

        return tarifas.Select(t => t.ToDto()).ToList();
    }

    public async Task<TarifaDto?> ObtenerPorIdAsync(int idTarifa, CancellationToken cancellationToken = default)
    {
        var tarifa = await _tarifas.ObtenerPorIdAsync(idTarifa, cancellationToken);

        return tarifa?.ToDto();
    }

    public async Task<int> CrearAsync(CrearTarifaCommand command, CancellationToken cancellationToken = default)
    {
        await ValidarReferenciasAsync(command.IdAlojamiento, command.IdTemporada, cancellationToken);

        var tarifa = Tarifa.Crear(
            command.IdAlojamiento,
            command.IdTemporada,
            command.PrecioBase,
            command.PrecioPersonaAdicional,
            command.Activa);

        _tarifas.Agregar(tarifa);
        await _unitOfWork.GuardarCambiosAsync(cancellationToken);

        return tarifa.IdTarifa;
    }

    public async Task ActualizarAsync(ActualizarTarifaCommand command, CancellationToken cancellationToken = default)
    {
        var tarifa = await ObtenerExistenteAsync(command.IdTarifa, cancellationToken);

        await ValidarReferenciasAsync(command.IdAlojamiento, command.IdTemporada, cancellationToken);

        tarifa.Actualizar(
            command.IdAlojamiento,
            command.IdTemporada,
            command.PrecioBase,
            command.PrecioPersonaAdicional,
            command.Activa);

        await _unitOfWork.GuardarCambiosAsync(cancellationToken);
    }

    public async Task EliminarAsync(int idTarifa, CancellationToken cancellationToken = default)
    {
        var tarifa = await ObtenerExistenteAsync(idTarifa, cancellationToken);

        _tarifas.Eliminar(tarifa);
        await _unitOfWork.GuardarCambiosAsync(cancellationToken);
    }

    private async Task<Tarifa> ObtenerExistenteAsync(int idTarifa, CancellationToken cancellationToken)
    {
        return await _tarifas.ObtenerPorIdAsync(idTarifa, cancellationToken)
            ?? throw new EntidadNoEncontradaException("La tarifa", idTarifa);
    }

    private async Task ValidarReferenciasAsync(int idAlojamiento, int idTemporada, CancellationToken cancellationToken)
    {
        if (!await _alojamientos.ExisteAsync(idAlojamiento, cancellationToken))
            throw new EntidadNoEncontradaException("El alojamiento", idAlojamiento);

        if (!await _temporadas.ExisteAsync(idTemporada, cancellationToken))
            throw new EntidadNoEncontradaException("La temporada", idTemporada);
    }
}
