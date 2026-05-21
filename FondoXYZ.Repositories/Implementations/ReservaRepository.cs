using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FondoXYZ.Data;
using FondoXYZ.Entities;
using FondoXYZ.Entities.DTOs;
using FondoXYZ.Repositories.Interfaces;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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

        // =========================
        // OBTENER RESERVAS
        // =========================

        public async Task<List<Reserva>> ObtenerReservas()
        {
            return await _context.Reservas
                .Include(x => x.Alojamiento)
                .ToListAsync();
        }

        // =========================
        // OBTENER RESERVA POR ID
        // =========================

        public async Task<Reserva?> ObtenerPorId(int id)
        {
            return await _context.Reservas
                .FirstOrDefaultAsync(x =>
                    x.IdReserva == id);
        }

        // =========================
        // CREAR RESERVA
        // =========================

        public async Task CrearReserva(
            Reserva reserva)
        {
            await _context.Reservas
                .AddAsync(reserva);

            await _context.SaveChangesAsync();
        }


        public async Task<bool> ExisteCruceReserva(
         int idAlojamiento,
         DateTime fechaInicio,
         DateTime fechaFin)
            {
            return await _context.Reservas.AnyAsync(r =>

                r.IdAlojamiento == idAlojamiento

                &&

                (
                    fechaInicio < r.FechaFin
                    &&
                    fechaFin > r.FechaInicio
                )
            );
        }
        // =========================
        // ACTUALIZAR RESERVA
        // =========================

        public async Task ActualizarReserva(
            Reserva reserva)
        {
            _context.Reservas
                .Update(reserva);

            await _context.SaveChangesAsync();
        }

        // =========================
        // ELIMINAR RESERVA
        // =========================

        public async Task EliminarReserva(int id)
        {
            var reserva =
                await ObtenerPorId(id);

            if (reserva != null)
            {
                _context.Reservas
                    .Remove(reserva);

                await _context.SaveChangesAsync();
            }
        }

        // =========================
        // CONSULTAR DISPONIBILIDAD
        // =========================

        public async Task<List<DisponibilidadDto>>
            ConsultarDisponibilidad(
                DateTime fechaInicio,
                DateTime fechaFin)
        {
            var parametros = new[]
            {
                new SqlParameter(
                    "@FechaInicio",
                    fechaInicio),

                new SqlParameter(
                    "@FechaFin",
                    fechaFin)
            };

            var resultado = await _context
                .Disponibilidades
                .FromSqlRaw(
                    @"EXEC sp_ConsultarDisponibilidad
                        @FechaInicio,
                        @FechaFin",
                    parametros)
                .ToListAsync();

            return resultado;
        }

        // =========================
        // CALCULAR TARIFA
        // =========================

        public async Task<CalculoTarifaDto>
            CalcularTarifa(
                int idAlojamiento,
                int numeroPersonas,
                DateTime fechaInicio,
                DateTime fechaFin)
        {
            var parametros = new[]
            {
                new SqlParameter(
                    "@IdAlojamiento",
                    idAlojamiento),

                new SqlParameter(
                    "@FechaInicio",
                    fechaInicio),

                new SqlParameter(
                    "@FechaFin",
                    fechaFin),

                new SqlParameter(
                    "@NumeroPersonas",
                    numeroPersonas)
            };

            var resultado = await _context
                .CalculoTarifas
                .FromSqlRaw(
                    @"EXEC sp_CalcularTarifa
                        @IdAlojamiento,
                        @FechaInicio,
                        @FechaFin,
                        @NumeroPersonas",
                    parametros)
                .ToListAsync();

            return resultado.FirstOrDefault()
                ?? new CalculoTarifaDto();
        }
    }
}