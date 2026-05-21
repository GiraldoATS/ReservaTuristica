using FondoXYZ.Entities;
using FondoXYZ.Entities.DTOs;

namespace FondoXYZ.Services.Interfaces
{
    public interface IReservaService
    {
        Task<List<Reserva>> ObtenerReservas();

        Task CrearReserva(Reserva reserva);

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
    }
}