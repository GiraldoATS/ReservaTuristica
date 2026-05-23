using FondoXYZ.Data;
using FondoXYZ.Entities;
using FondoXYZ.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Repositories.Implementations
{
    public class SedeRepository : ISedeRepository
    {
        private readonly ApplicationDbContext _context;

        public SedeRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sede>> ObtenerSedes()
        {
            return await _context.Sedes
                .ToListAsync();
        }

        public async Task Crear(Sede sede)
        {
            _context.Sedes.Add(sede);

            await _context.SaveChangesAsync();
        }
    }
}