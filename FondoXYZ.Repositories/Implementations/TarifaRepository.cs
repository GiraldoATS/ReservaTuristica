using FondoXYZ.Data;
using FondoXYZ.Entities;
using FondoXYZ.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FondoXYZ.Repositories.Implementations
{
    public class TarifaRepository
        : ITarifaRepository
    {
        private readonly ApplicationDbContext _context;

        public TarifaRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tarifa>>
            ObtenerTarifas()
        {
            return await _context.Tarifas
                .Include(x => x.Alojamiento)
                .Include(x => x.Temporada)
                .ToListAsync();
        }

        public async Task<Tarifa?>
            ObtenerPorId(int id)
        {
            return await _context.Tarifas
                .FirstOrDefaultAsync(
                    x => x.IdTarifa == id);
        }

        public async Task CrearTarifa(
            Tarifa tarifa)
        {
            await _context.Tarifas
                .AddAsync(tarifa);

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarTarifa(
            Tarifa tarifa)
        {
            _context.Tarifas.Update(tarifa);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarTarifa(int id)
        {
            var tarifa =
                await ObtenerPorId(id);

            if (tarifa != null)
            {
                _context.Tarifas.Remove(tarifa);

                await _context.SaveChangesAsync();
            }
        }
    }
}