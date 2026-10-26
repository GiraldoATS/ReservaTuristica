using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Infrastructure.Persistence.Repositories;

internal sealed class AlojamientoRepository : IAlojamientoRepository
{
    private readonly ApplicationDbContext _context;

    public AlojamientoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Alojamiento>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Alojamientos
            .AsNoTracking()
            .Include(x => x.Sede)
            .OrderBy(x => x.Nombre)
            .ToListAsync(cancellationToken);
    }

    public Task<Alojamiento?> ObtenerPorIdAsync(int idAlojamiento, CancellationToken cancellationToken = default) =>
        _context.Alojamientos.FirstOrDefaultAsync(x => x.IdAlojamiento == idAlojamiento, cancellationToken);

    public Task<bool> ExisteAsync(int idAlojamiento, CancellationToken cancellationToken = default) =>
        _context.Alojamientos.AnyAsync(x => x.IdAlojamiento == idAlojamiento, cancellationToken);

    public void Agregar(Alojamiento alojamiento) => _context.Alojamientos.Add(alojamiento);

    public void Eliminar(Alojamiento alojamiento) => _context.Alojamientos.Remove(alojamiento);
}
