using FondoXYZ.Entities;

namespace FondoXYZ.Repositories.Interfaces
{
    public interface IAlojamientoRepository
    {
        Task<List<Alojamiento>> ObtenerAlojamientos();

        Task<Alojamiento?> ObtenerPorId(int id);

        Task Crear(Alojamiento alojamiento);

        Task Actualizar(Alojamiento alojamiento);

        Task Eliminar(int id);
    }
}