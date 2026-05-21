using FondoXYZ.Entities;

namespace FondoXYZ.Repositories.Interfaces
{
    public interface IAlojamientoRepository
    {
        Task<List<Alojamiento>>
            ObtenerAlojamientos();

        Task<Alojamiento?> ObtenerPorId(int id);
    }
}