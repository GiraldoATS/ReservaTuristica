using FondoXYZ.Entities;
using FondoXYZ.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FondoXYZ.Web.Controllers
{
    public class TarifasController : Controller
    {
        private readonly ITarifaService _service;

        private readonly IAlojamientoService
            _alojamientoService;

        private readonly ITemporadaService
            _temporadaService;

        public TarifasController(
            ITarifaService service,
            IAlojamientoService alojamientoService,
            ITemporadaService temporadaService)
        {
            _service = service;

            _alojamientoService =
                alojamientoService;

            _temporadaService =
                temporadaService;
        }

        #region Listado

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tarifas =
                await _service.ObtenerTarifas();

            return View(tarifas);
        }

        #endregion

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CargarCombos();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Tarifa tarifa)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CargarCombos();

                    return View(tarifa);
                }

                await _service.CrearTarifa(tarifa);

                TempData["success"] =
                    "Tarifa creada correctamente";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                await CargarCombos();

                return View(tarifa);
            }
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id)
        {
            var tarifa =
                await _service.ObtenerPorId(id);

            if (tarifa == null)
            {
                return NotFound();
            }

            await CargarCombos();

            return View(tarifa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Tarifa tarifa)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CargarCombos();

                    return View(tarifa);
                }

                await _service
                    .ActualizarTarifa(tarifa);

                TempData["success"] =
                    "Tarifa actualizada correctamente";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                await CargarCombos();

                return View(tarifa);
            }
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            int id)
        {
            await _service.EliminarTarifa(id);

            TempData["success"] =
                "Tarifa eliminada correctamente";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Helpers

        private async Task CargarCombos()
        {
            var alojamientos =
                await _alojamientoService
                    .ObtenerAlojamientos();

            var temporadas =
                await _temporadaService
                    .ObtenerTemporadas();

            ViewBag.Alojamientos =
                new SelectList(
                    alojamientos,
                    "IdAlojamiento",
                    "Nombre");

            ViewBag.Temporadas =
                new SelectList(
                    temporadas,
                    "IdTemporada",
                    "Nombre");
        }

        #endregion
    }
}