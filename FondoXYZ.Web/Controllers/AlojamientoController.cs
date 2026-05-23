using FondoXYZ.Entities;
using FondoXYZ.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FondoXYZ.Web.Controllers
{
    public class AlojamientoController : Controller
    {
        private readonly IAlojamientoService _service;

        private readonly ISedeService _sedeService;

        public AlojamientoController(
            IAlojamientoService service,
            ISedeService sedeService)
        {
            _service = service;

            _sedeService = sedeService;
        }

        #region Listado

        public async Task<IActionResult> Index()
        {
            var alojamientos =
                await _service.ObtenerAlojamientos();

            return View(alojamientos);
        }

        #endregion

        #region Create

        public async Task<IActionResult> Create()
        {
            await CargarSedes();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Alojamiento model)
        {
            if (!ModelState.IsValid)
            {
                await CargarSedes();

                return View(model);
            }

            model.Estado = true;

            await _service.Crear(model);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        public async Task<IActionResult> Edit(int id)
        {
            var alojamiento =
                await _service.ObtenerPorId(id);

            if (alojamiento == null)
            {
                return NotFound();
            }

            await CargarSedes();

            return View(alojamiento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Alojamiento model)
        {
            if (!ModelState.IsValid)
            {
                await CargarSedes();

                return View(model);
            }

            await _service.Actualizar(model);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(int id)
        {
            await _service.Eliminar(id);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Helpers

        private async Task CargarSedes()
        {
            var sedes =
                await _sedeService.ObtenerSedes();

            ViewBag.Sedes = new SelectList(
                sedes,
                "IdSede",
                "Nombre");
        }

        #endregion
    }
}