using FondoXYZ.Data;
using FondoXYZ.Entities;
using FondoXYZ.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Repositories.Implementations
{
    public class AlojamientoRepository
        : IAlojamientoRepository
    {
        private readonly ApplicationDbContext _context;

        public AlojamientoRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Alojamiento>>
            ObtenerAlojamientos()
        {
            return await _context.Alojamientos
                .ToListAsync();
        }
        public async Task<Alojamiento?>
            ObtenerPorId(int id)
        {
            return await _context.Alojamientos
                .FirstOrDefaultAsync(x =>
                    x.IdAlojamiento == id);
        }
    }
}