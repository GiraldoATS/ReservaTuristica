using FondoXYZ.Data;
using FondoXYZ.Entities.DTOs;
using FondoXYZ.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Repositories.Implementations
{
    public class DashboardRepository
        : IDashboardRepository
    {
        private readonly ApplicationDbContext
            _context;

        public DashboardRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto>
            ObtenerDashboard()
        {
            var dashboard =
                new DashboardDto();

            dashboard.TotalReservas =
                await _context.Reservas
                    .CountAsync();

            dashboard.IngresosTotales =
                await _context.Reservas
                    .SumAsync(x => (decimal?)x.Total)
                    ?? 0;

            dashboard.TotalAlojamientos =
                await _context.Alojamientos
                    .CountAsync();

            dashboard.TarifasActivas =
                await _context.Tarifas
                    .CountAsync(x => x.Estado);

            dashboard.UltimasReservas =
                await _context.Reservas
                    .Include(x => x.Alojamiento)
                    .OrderByDescending(x => x.IdReserva)
                    .Take(5)
                    .ToListAsync();

            return dashboard;
        }
    }
}