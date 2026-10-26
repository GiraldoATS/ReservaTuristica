using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Infrastructure.Persistence.Repositories;

internal sealed class SedeRepository : ISedeRepository
{
    private readonly ApplicationDbContext _context;

    public SedeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Sede>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Sedes
            .AsNoTracking()
            .OrderBy(x => x.Nombre)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExisteAsync(int idSede, CancellationToken cancellationToken = default) =>
        _context.Sedes.AnyAsync(x => x.IdSede == idSede, cancellationToken);

    public void Agregar(Sede sede) => _context.Sedes.Add(sede);
}
