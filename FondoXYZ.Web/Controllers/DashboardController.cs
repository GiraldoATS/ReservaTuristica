using FondoXYZ.Application.Common;
using FondoXYZ.Application.Ports.In;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FondoXYZ.Web.Controllers;

[Authorize(Roles = Roles.Admin)]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
        View(await _dashboardService.ObtenerAsync(cancellationToken));
}
