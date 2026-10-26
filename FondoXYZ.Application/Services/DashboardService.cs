using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Ports.In;
using FondoXYZ.Application.Ports.Out;

namespace FondoXYZ.Application.Services;

internal sealed class DashboardService : IDashboardService
{
    private readonly IDashboardQuery _dashboard;

    public DashboardService(IDashboardQuery dashboard)
    {
        _dashboard = dashboard;
    }

    public Task<DashboardDto> ObtenerAsync(CancellationToken cancellationToken = default) =>
        _dashboard.ObtenerAsync(cancellationToken);
}
