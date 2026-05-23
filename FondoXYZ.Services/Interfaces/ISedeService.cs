using FondoXYZ.Entities;

namespace FondoXYZ.Services.Interfaces
{
    public interface ISedeService
    {
        Task<List<Sede>> ObtenerSedes();

        Task Crear(Sede sede);
    }
}