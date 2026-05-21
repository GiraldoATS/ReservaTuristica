using FondoXYZ.Entities.DTOs;
using FondoXYZ.Repositories.Interfaces;
using FondoXYZ.Services.Interfaces;

namespace FondoXYZ.Services.Implementations
{
    public class DashboardService
        : IDashboardService
    {
        private readonly IDashboardRepository
            _repository;

        public DashboardService(
            IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<DashboardDto>
            ObtenerDashboard()
        {
            return await _repository
                .ObtenerDashboard();
        }
    }
}