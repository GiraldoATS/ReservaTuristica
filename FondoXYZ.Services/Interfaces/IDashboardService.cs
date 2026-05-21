using FondoXYZ.Entities.DTOs;

namespace FondoXYZ.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> ObtenerDashboard();
    }
}