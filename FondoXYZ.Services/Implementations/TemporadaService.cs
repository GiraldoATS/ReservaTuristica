using FondoXYZ.Entities;
using FondoXYZ.Repositories.Interfaces;
using FondoXYZ.Services.Interfaces;

namespace FondoXYZ.Services.Implementations
{
    public class TemporadaService
        : ITemporadaService
    {
        private readonly ITemporadaRepository _repository;

        public TemporadaService(
            ITemporadaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Temporada>>
            ObtenerTemporadas()
        {
            return await _repository
                .ObtenerTemporadas();
        }
    }
}