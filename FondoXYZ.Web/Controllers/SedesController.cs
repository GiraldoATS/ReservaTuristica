using FondoXYZ.Entities;
using FondoXYZ.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FondoXYZ.Web.Controllers
{
    public class SedesController : Controller
    {
        private readonly ISedeService _service;

        public SedesController(
            ISedeService service)
        {
            _service = service;
        }

        // LISTADO
        public async Task<IActionResult> Index()
        {
            var sedes =
                await _service.ObtenerSedes();

            return View(sedes);
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        public async Task<IActionResult> Create(
            Sede model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _service.Crear(model);

            return RedirectToAction(nameof(Index));
        }
    }
}