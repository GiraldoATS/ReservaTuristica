using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Mappings;
using FondoXYZ.Application.Ports.In;
using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.Entities;

namespace FondoXYZ.Application.Services;

internal sealed class SedeService : ISedeService
{
    private readonly ISedeRepository _sedes;
    private readonly IUnitOfWork _unitOfWork;

    public SedeService(ISedeRepository sedes, IUnitOfWork unitOfWork)
    {
        _sedes = sedes;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<SedeDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        var sedes = await _sedes.ObtenerTodasAsync(cancellationToken);

        return sedes.Select(s => s.ToDto()).ToList();
    }

    public async Task<int> CrearAsync(CrearSedeCommand command, CancellationToken cancellationToken = default)
    {
        var sede = Sede.Crear(
            command.Nombre,
            command.Ciudad,
            command.Tipo,
            command.Descripcion,
            command.CapacidadTotal);

        _sedes.Agregar(sede);
        await _unitOfWork.GuardarCambiosAsync(cancellationToken);

        return sede.IdSede;
    }
}
