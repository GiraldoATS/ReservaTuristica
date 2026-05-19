using FondoXYZ.Entities;
using FondoXYZ.Entities.DTOs;
using FondoXYZ.Repositories.Interfaces;
using FondoXYZ.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FondoXYZ.Services.Implementations
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repository;

        public ReservaService(
            IReservaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Reserva>> ObtenerReservas()
        {
            return await _repository.ObtenerReservas();
        }

        public async Task CrearReserva(Reserva reserva)
        {
            // LOGICA NEGOCIO

            if (reserva.FechaFin <
                reserva.FechaInicio)
            {
                throw new Exception(
                    "La fecha final no puede ser menor");
            }

            await _repository.CrearReserva(reserva);
        }
        public async Task<List<DisponibilidadDto>>
         ConsultarDisponibilidad(DateTime fechaInicio,DateTime fechaFin)
        {
            return await _repository
                .ConsultarDisponibilidad(fechaInicio,fechaFin);
        }
    }
}
