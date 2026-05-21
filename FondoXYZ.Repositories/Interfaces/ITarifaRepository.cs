using FondoXYZ.Entities;

namespace FondoXYZ.Repositories.Interfaces
{
    public interface ITarifaRepository
    {
        Task<List<Tarifa>> ObtenerTarifas();

        Task<Tarifa?> ObtenerPorId(int id);

        Task CrearTarifa(Tarifa tarifa);

        Task ActualizarTarifa(Tarifa tarifa);

        Task EliminarTarifa(int id);
    }
}