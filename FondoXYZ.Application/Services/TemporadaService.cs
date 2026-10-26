using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Mappings;
using FondoXYZ.Application.Ports.In;
using FondoXYZ.Application.Ports.Out;

namespace FondoXYZ.Application.Services;

internal sealed class TemporadaService : ITemporadaService
{
    private readonly ITemporadaRepository _temporadas;

    public TemporadaService(ITemporadaRepository temporadas)
    {
        _temporadas = temporadas;
    }

    public async Task<IReadOnlyList<TemporadaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        var temporadas = await _temporadas.ObtenerTodasAsync(cancellationToken);

        return temporadas.Select(t => t.ToDto()).ToList();
    }
}
