using FondoXYZ.Entities;

namespace FondoXYZ.Services.Interfaces
{
    public interface IAlojamientoService
    {
        Task<List<Alojamiento>>
            ObtenerAlojamientos();
        Task<Alojamiento?> ObtenerPorId(int id);

        Task Crear(Alojamiento alojamiento);

        Task Actualizar(Alojamiento alojamiento);

        Task Eliminar(int id);
    }
}