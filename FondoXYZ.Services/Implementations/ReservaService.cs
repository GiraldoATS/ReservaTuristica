using FondoXYZ.Entities;
using FondoXYZ.Entities.DTOs;
using FondoXYZ.Repositories.Interfaces;
using FondoXYZ.Services.Interfaces;

namespace FondoXYZ.Services.Implementations
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repository;

        private readonly IAlojamientoRepository
            _alojamientoRepository;

        public ReservaService(
            IReservaRepository repository,
            IAlojamientoRepository alojamientoRepository)
        {
            _repository = repository;

            _alojamientoRepository =
                alojamientoRepository;
        }

        #region Reservas

        public async Task<List<Reserva>>
            ObtenerReservas()
        {
            return await _repository
                .ObtenerReservas();
        }

        public async Task CrearReserva(
            Reserva reserva)
        {
            ValidarFechas(
                reserva.FechaInicio,
                reserva.FechaFin);

            await ValidarCruceReserva(reserva);

            var alojamiento =
                await ValidarAlojamiento(
                    reserva.IdAlojamiento);

            ValidarCapacidad(
                reserva.NumeroPersonas,
                alojamiento.CapacidadMaxima);

            await _repository
                .CrearReserva(reserva);
        }

        #endregion

        #region Disponibilidad

        public async Task<List<DisponibilidadDto>>
            ConsultarDisponibilidad(
                DateTime fechaInicio,
                DateTime fechaFin)
        {
            return await _repository
                .ConsultarDisponibilidad(
                    fechaInicio,
                    fechaFin);
        }

        #endregion

        #region Tarifas

        public async Task<CalculoTarifaDto>
            CalcularTarifa(
                int idAlojamiento,
                int numeroPersonas,
                DateTime fechaInicio,
                DateTime fechaFin)
        {
            ValidarFechas(
                fechaInicio,
                fechaFin);

            return await _repository
                .CalcularTarifa(
                    idAlojamiento,
                    numeroPersonas,
                    fechaInicio,
                    fechaFin);
        }

        #endregion

        #region Validaciones

        private static void ValidarFechas(
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            if (fechaFin <= fechaInicio)
            {
                throw new Exception(
                    "La fecha final debe ser mayor");
            }
        }

        private async Task ValidarCruceReserva(
            Reserva reserva)
        {
            bool existeCruce =
                await _repository
                    .ExisteCruceReserva(
                        reserva.IdAlojamiento,
                        reserva.FechaInicio,
                        reserva.FechaFin);

            if (existeCruce)
            {
                throw new Exception(
                    "Ya existe una reserva para este alojamiento en las fechas seleccionadas");
            }
        }

        private async Task<Alojamiento>
            ValidarAlojamiento(
                int idAlojamiento)
        {
            var alojamiento =
                await _alojamientoRepository
                    .ObtenerPorId(idAlojamiento);

            if (alojamiento == null)
            {
                throw new Exception(
                    "El alojamiento no existe");
            }

            return alojamiento;
        }

        private static void ValidarCapacidad(
            int numeroPersonas,
            int capacidadMaxima)
        {
            if (numeroPersonas > capacidadMaxima)
            {
                throw new Exception(
                    $"La capacidad máxima es de {capacidadMaxima} personas");
            }
        }

        #endregion
    }
}