using FondoXYZ.Entities;
using FondoXYZ.Entities.DTOs;
using FondoXYZ.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FondoXYZ.Web.Controllers
{
    public class ReservasController : Controller
    {
        private readonly IReservaService _service;

        public ReservasController(
            IReservaService service)
        {
            _service = service;
        }

        // =========================
        // LISTAR RESERVAS
        // =========================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reservas =
                await _service.ObtenerReservas();

            return View(reservas);
        }

        // =========================
        // CREAR RESERVA
        // =========================
        public IActionResult Create(
            int id)
        {
            var reserva = new Reserva
            {
                IdAlojamiento = id,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(1)
            };

            return View(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
     Reserva reserva)
        {
            try
            {
                // =========================
                // USUARIO TEMPORAL
                // =========================

                reserva.IdUsuario = "USUARIO_TEST";

                // =========================
                // LIMPIAR ERROR MVC
                // =========================

                ModelState.Remove("IdUsuario");

                // =========================
                // VALIDACIONES
                // =========================

                if (!ModelState.IsValid)
                {
                    return View(reserva);
                }

                // =========================
                // GUARDAR
                // =========================

                await _service.CrearReserva(reserva);

                TempData["success"] =
                    "Reserva creada correctamente";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(reserva);
            }
        }
        // =========================
        // CONSULTAR DISPONIBILIDAD
        // =========================

        [HttpGet]
        public IActionResult Disponibilidad()
        {
            return View(
                new List<DisponibilidadDto>());
        }

        [HttpPost]
        public async Task<IActionResult>
            Disponibilidad(
                DateTime fechaInicio,
                DateTime fechaFin)
        {
            var resultado =
                await _service
                    .ConsultarDisponibilidad(
                        fechaInicio,
                        fechaFin);

            return View(resultado);
        }

        // =========================
        // CALCULAR TARIFA
        // =========================

        [HttpPost]
        public async Task<JsonResult> CalcularTarifa(
    [FromBody] CalcularTarifaRequest request)
        {
            try
            {
                var resultado =
                    await _service.CalcularTarifa(
                        request.IdAlojamiento,
                        request.NumeroPersonas,
                        request.FechaInicio,
                        request.FechaFin);

                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    error = ex.Message
                });
            }
        }
    }
}