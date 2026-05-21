using FondoXYZ.Entities;
using FondoXYZ.Entities.DTOs;

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

        Task<CalculoTarifaDto>
            CalcularTarifa(
                int idAlojamiento,
                int numeroPersonas,
                DateTime fechaInicio,
                DateTime fechaFin);
        Task<bool> ExisteCruceReserva(
             int idAlojamiento,
            DateTime fechaInicio,
            DateTime fechaFin);
    }
}