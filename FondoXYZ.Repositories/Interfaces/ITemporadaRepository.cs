using FondoXYZ.Entities;

namespace FondoXYZ.Repositories.Interfaces
{
    public interface ITemporadaRepository
    {
        Task<List<Temporada>> ObtenerTemporadas();
    }
}