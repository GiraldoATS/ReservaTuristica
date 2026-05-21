using FondoXYZ.Data;
using FondoXYZ.Entities;
using FondoXYZ.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Repositories.Implementations
{
    public class TemporadaRepository
        : ITemporadaRepository
    {
        private readonly ApplicationDbContext _context;

        public TemporadaRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Temporada>>
            ObtenerTemporadas()
        {
            return await _context.Temporadas
                .ToListAsync();
        }
    }
}