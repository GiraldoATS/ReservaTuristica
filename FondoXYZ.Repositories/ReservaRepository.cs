using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FondoXYZ.Data;
using FondoXYZ.Entities;
using FondoXYZ.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using FondoXYZ.Entities.DTOs;
using Microsoft.Data.SqlClient;


namespace FondoXYZ.Repositories.Implementations
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservaRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reserva>> ObtenerReservas()
        {
            return await _context.Reservas
                .Include(x => x.Alojamiento)
                .ToListAsync();
        }

        public async Task<Reserva?> ObtenerPorId(int id)
        {
            return await _context.Reservas
                .FirstOrDefaultAsync(x =>
                    x.IdReserva == id);
        }

        public async Task CrearReserva(Reserva reserva)
        {
            await _context.Reservas.AddAsync(reserva);

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarReserva(
            Reserva reserva)
        {
            _context.Reservas.Update(reserva);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarReserva(int id)
        {
            var reserva =
                await ObtenerPorId(id);

            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);

                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<DisponibilidadDto>>
         ConsultarDisponibilidad(
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            var parametros = new[]
            {
            new SqlParameter("@FechaInicio", fechaInicio),
            new SqlParameter("@FechaFin", fechaFin)
    };

                 var resultado = await _context
                .Set<DisponibilidadDto>()
                .FromSqlRaw(
                    "EXEC sp_ConsultarDisponibilidad @FechaInicio, @FechaFin",
                    parametros)
                .ToListAsync();

                return resultado;
        }
    }
}