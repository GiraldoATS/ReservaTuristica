using FondoXYZ.Entities;

namespace FondoXYZ.Services.Interfaces
{
    public interface ITarifaService
    {
        Task<List<Tarifa>> ObtenerTarifas();

        Task<Tarifa?> ObtenerPorId(int id);

        Task CrearTarifa(Tarifa tarifa);

        Task ActualizarTarifa(Tarifa tarifa);

        Task EliminarTarifa(int id);
    }
}