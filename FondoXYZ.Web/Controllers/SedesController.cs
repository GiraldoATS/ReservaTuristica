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

        #region Listado

        public async Task<IActionResult> Index()
        {
            var sedes =
                await _service.ObtenerSedes();

            return View(sedes);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Sede model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _service.Crear(model);

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}