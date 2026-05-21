using FondoXYZ.Entities.DTOs;

namespace FondoXYZ.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardDto> ObtenerDashboard();
    }
}