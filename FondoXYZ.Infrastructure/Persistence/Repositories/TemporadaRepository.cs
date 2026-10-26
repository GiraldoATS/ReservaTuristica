using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Infrastructure.Persistence.Repositories;

internal sealed class TemporadaRepository : ITemporadaRepository
{
    private readonly ApplicationDbContext _context;

    public TemporadaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Temporada>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Temporadas
            .AsNoTracking()
            .OrderBy(x => x.FechaInicio)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExisteAsync(int idTemporada, CancellationToken cancellationToken = default) =>
        _context.Temporadas.AnyAsync(x => x.IdTemporada == idTemporada, cancellationToken);
}
