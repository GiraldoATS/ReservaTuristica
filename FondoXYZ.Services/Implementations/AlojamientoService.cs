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

        #region Alojamiento

        public async Task<List<Alojamiento>>
            ObtenerAlojamientos()
        {
            return await _repository
                .ObtenerAlojamientos();
        }

        public async Task<Alojamiento?>
            ObtenerPorId(int id)
        {
            return await _repository
                .ObtenerPorId(id);
        }

        public async Task Crear(
            Alojamiento alojamiento)
        {
            await _repository
                .Crear(alojamiento);
        }

        public async Task Actualizar(
            Alojamiento alojamiento)
        {
            await _repository
                .Actualizar(alojamiento);
        }

        public async Task Eliminar(int id)
        {
            await _repository
                .Eliminar(id);
        }

        #endregion
    }
}