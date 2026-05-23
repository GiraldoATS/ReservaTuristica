using FondoXYZ.Entities;
using FondoXYZ.Repositories.Interfaces;
using FondoXYZ.Services.Interfaces;

namespace FondoXYZ.Services.Implementations
{
    public class SedeService : ISedeService
    {
        private readonly ISedeRepository _repository;

        public SedeService(
            ISedeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Sede>> ObtenerSedes()
        {
            return await _repository
                .ObtenerSedes();
        }

        public async Task Crear(Sede sede)
        {
            await _repository.Crear(sede);
        }
    }
}