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

        // =========================
        // OBTENER RESERVAS
        // =========================

        public async Task<List<Reserva>>
            ObtenerReservas()
        {
            return await _repository
                .ObtenerReservas();
        }

        // =========================
        // CREAR RESERVA
        // =========================

        public async Task CrearReserva(
            Reserva reserva)
        {
            // =========================
            // VALIDAR FECHAS
            // =========================

            if (reserva.FechaFin <=
                reserva.FechaInicio)
            {
                throw new Exception(
                    "La fecha final debe ser mayor");
            }

            // =========================
            // VALIDAR CRUCE
            // =========================

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

            // =========================
            // VALIDAR ALOJAMIENTO
            // =========================

            var alojamiento =
                await _alojamientoRepository
                    .ObtenerPorId(
                        reserva.IdAlojamiento);

            if (alojamiento == null)
            {
                throw new Exception(
                    "El alojamiento no existe");
            }

            // =========================
            // VALIDAR CAPACIDAD
            // =========================

            if (reserva.NumeroPersonas >
                alojamiento.CapacidadMaxima)
            {
                throw new Exception(
                    $"La capacidad máxima es de {alojamiento.CapacidadMaxima} personas");
            }

            // =========================
            // GUARDAR
            // =========================

            await _repository
                .CrearReserva(reserva);
        }

        // =========================
        // CONSULTAR DISPONIBILIDAD
        // =========================

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
            if (fechaFin <= fechaInicio)
            {
                throw new Exception(
                    "La fecha final debe ser mayor");
            }

            return await _repository
                .CalcularTarifa(
                    idAlojamiento,
                    numeroPersonas,
                    fechaInicio,
                    fechaFin);
        }
    }
}