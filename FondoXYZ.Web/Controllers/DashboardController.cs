using FondoXYZ.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FondoXYZ.Web.Controllers
{
    public class DashboardController
        : Controller
    {
        private readonly IDashboardService
            _service;

        public DashboardController(
            IDashboardService service)
        {
            _service = service;
        }

        public async Task<IActionResult>
            Index()
        {
            var dashboard =
                await _service
                    .ObtenerDashboard();

            return View(dashboard);
        }
    }
}