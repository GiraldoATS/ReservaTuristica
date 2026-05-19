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

        public async Task<IActionResult> Index()
        {
            var reservas =
                await _service.ObtenerReservas();

            return View(reservas);
        }

        // =========================
        // CREAR RESERVA
        // =========================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                return View(reserva);
            }

            await _service.CrearReserva(reserva);

            TempData["success"] =
                "Reserva creada correctamente";

            return RedirectToAction(nameof(Index));
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
    }
}