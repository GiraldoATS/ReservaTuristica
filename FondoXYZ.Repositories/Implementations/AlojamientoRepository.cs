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
                 .Include(x => x.Sede)
                 .ToListAsync();
        }
        public async Task<Alojamiento?>
          ObtenerPorId(int id)
        {
            return await _context.Alojamientos
                .FirstOrDefaultAsync(x =>
                    x.IdAlojamiento == id);
        }
        public async Task Crear(Alojamiento alojamiento)
        {
            _context.Alojamientos.Add(alojamiento);

            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Alojamiento alojamiento)
        {
            _context.Alojamientos.Update(alojamiento);

            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var alojamiento = await _context.Alojamientos
                .FirstOrDefaultAsync(x => x.IdAlojamiento == id);

            if (alojamiento != null)
            {
                _context.Alojamientos.Remove(alojamiento);

                await _context.SaveChangesAsync();
            }
        }
    }
}