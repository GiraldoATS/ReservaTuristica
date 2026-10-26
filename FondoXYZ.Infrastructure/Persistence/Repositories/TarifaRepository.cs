using FondoXYZ.Application.Ports.Out;
using FondoXYZ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Infrastructure.Persistence.Repositories;

internal sealed class TarifaRepository : ITarifaRepository
{
    private readonly ApplicationDbContext _context;

    public TarifaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Tarifa>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tarifas
            .AsNoTracking()
            .Include(x => x.Alojamiento)
            .Include(x => x.Temporada)
            .OrderBy(x => x.IdTarifa)
            .ToListAsync(cancellationToken);
    }

    public Task<Tarifa?> ObtenerPorIdAsync(int idTarifa, CancellationToken cancellationToken = default) =>
        _context.Tarifas.FirstOrDefaultAsync(x => x.IdTarifa == idTarifa, cancellationToken);

    public Task<bool> ExisteParaAlojamientoAsync(int idAlojamiento, CancellationToken cancellationToken = default) =>
        _context.Tarifas.AnyAsync(x => x.IdAlojamiento == idAlojamiento, cancellationToken);

    public void Agregar(Tarifa tarifa) => _context.Tarifas.Add(tarifa);

    public void Eliminar(Tarifa tarifa) => _context.Tarifas.Remove(tarifa);
}
