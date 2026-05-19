using FondoXYZ.Entities;
using FondoXYZ.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FondoXYZ.Repositories.Interfaces
{
    public interface IReservaRepository
    {
        Task<List<Reserva>> ObtenerReservas();

        Task<Reserva?> ObtenerPorId(int id);

        Task CrearReserva(Reserva reserva);

        Task ActualizarReserva(Reserva reserva);

        Task EliminarReserva(int id);

        Task<List<DisponibilidadDto>>
    ConsultarDisponibilidad(
        DateTime fechaInicio,
        DateTime fechaFin);
    }
}