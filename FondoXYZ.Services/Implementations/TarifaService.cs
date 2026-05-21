using FondoXYZ.Entities;
using FondoXYZ.Repositories.Interfaces;
using FondoXYZ.Services.Interfaces;

namespace FondoXYZ.Services.Implementations
{
    public class TarifaService
        : ITarifaService
    {
        private readonly ITarifaRepository _repository;

        public TarifaService(
            ITarifaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Tarifa>>
            ObtenerTarifas()
        {
            return await _repository
                .ObtenerTarifas();
        }

        public async Task<Tarifa?>
            ObtenerPorId(int id)
        {
            return await _repository
                .ObtenerPorId(id);
        }

        public async Task CrearTarifa(
            Tarifa tarifa)
        {
            await _repository
                .CrearTarifa(tarifa);
        }

        public async Task ActualizarTarifa(
            Tarifa tarifa)
        {
            await _repository
                .ActualizarTarifa(tarifa);
        }

        public async Task EliminarTarifa(
            int id)
        {
            await _repository
                .EliminarTarifa(id);
        }
    }
}