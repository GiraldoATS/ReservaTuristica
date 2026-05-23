using FondoXYZ.Entities;
using FondoXYZ.Repositories.Interfaces;
using FondoXYZ.Services.Interfaces;

namespace FondoXYZ.Services.Implementations
{
    public class AlojamientoService
        : IAlojamientoService
    {
        private readonly IAlojamientoRepository _repository;

        public AlojamientoService(
            IAlojamientoRepository repository)
        {
            _repository = repository;
        }

        // =========================
        // OBTENER TODOS
        // =========================

        public async Task<List<Alojamiento>>
            ObtenerAlojamientos()
        {
            return await _repository
                .ObtenerAlojamientos();
        }

        // =========================
        // OBTENER POR ID
        // =========================

        public async Task<Alojamiento?>
            ObtenerPorId(int id)
        {
            return await _repository
                .ObtenerPorId(id);
        }

        // =========================
        // CREAR
        // =========================

        public async Task Crear(
            Alojamiento alojamiento)
        {
            await _repository
                .Crear(alojamiento);
        }

        // =========================
        // ACTUALIZAR
        // =========================

        public async Task Actualizar(
            Alojamiento alojamiento)
        {
            await _repository
                .Actualizar(alojamiento);
        }

        // =========================
        // ELIMINAR
        // =========================

        public async Task Eliminar(int id)
        {
            await _repository
                .Eliminar(id);
        }
    }
}