using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Mappings;
using FondoXYZ.Application.Ports.Out;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Infrastructure.Persistence.Queries;

internal sealed class DashboardQuery : IDashboardQuery
{
    private const int CantidadUltimasReservas = 5;

    private readonly ApplicationDbContext _context;

    public DashboardQuery(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> ObtenerAsync(CancellationToken cancellationToken = default)
    {
        // DbContext no admite consultas en paralelo, por eso se ejecutan en secuencia.
        var totalReservas = await _context.Reservas.CountAsync(cancellationToken);

        var ingresosTotales = await _context.Reservas
            .SumAsync(x => (decimal?)x.Total, cancellationToken) ?? 0;

        var totalAlojamientos = await _context.Alojamientos.CountAsync(cancellationToken);

        var tarifasActivas = await _context.Tarifas.CountAsync(x => x.Activa, cancellationToken);

        var ultimasReservas = await _context.Reservas
            .AsNoTracking()
            .Include(x => x.Alojamiento)
            .OrderByDescending(x => x.IdReserva)
            .Take(CantidadUltimasReservas)
            .ToListAsync(cancellationToken);

        return new DashboardDto(
            totalReservas,
            ingresosTotales,
            totalAlojamientos,
            tarifasActivas,
            ultimasReservas.Select(r => r.ToDto()).ToList());
    }
}
