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

        public async Task<List<Alojamiento>>
            ObtenerAlojamientos()
        {
            return await _repository
                .ObtenerAlojamientos();
        }
    }
}