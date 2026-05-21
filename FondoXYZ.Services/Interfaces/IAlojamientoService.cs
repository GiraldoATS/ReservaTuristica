using FondoXYZ.Entities;

namespace FondoXYZ.Services.Interfaces
{
    public interface IAlojamientoService
    {
        Task<List<Alojamiento>>
            ObtenerAlojamientos();
    }
}