using FondoXYZ.Entities;

namespace FondoXYZ.Services.Interfaces
{
    public interface ITemporadaService
    {
        Task<List<Temporada>> ObtenerTemporadas();
    }
}