using FondoXYZ.Entities;

namespace FondoXYZ.Repositories.Interfaces
{
    public interface ISedeRepository
    {
        Task<List<Sede>> ObtenerSedes();

        Task Crear(Sede sede);
    }
}