using FondoXYZ.Entities;
using FondoXYZ.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FondoXYZ.Services.Interfaces
{
    public interface IReservaService
    {
        Task<List<Reserva>> ObtenerReservas();

        Task CrearReserva(Reserva reserva);
        Task<List<DisponibilidadDto>>
        ConsultarDisponibilidad(DateTime fechaInicio,DateTime fechaFin);

    }
}