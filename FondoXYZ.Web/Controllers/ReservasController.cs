using FondoXYZ.Entities;
using FondoXYZ.Entities.DTOs;
using FondoXYZ.Entities.ViewModels;
using FondoXYZ.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace FondoXYZ.Web.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly IReservaService _service;

        private readonly IAlojamientoService
            _alojamientoService;

        public ReservasController(
            IReservaService service,
            IAlojamientoService alojamientoService)
        {
            _service = service;

            _alojamientoService =
                alojamientoService;
        }

        #region Reservas

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reservas =
                await _service.ObtenerReservas();

            return View(reservas);
        }

        #endregion

        #region Create

        [HttpGet]
        public async Task<IActionResult>
            Create(int id)
        {
            var alojamiento =
                await _alojamientoService
                    .ObtenerPorId(id);

            if (alojamiento == null)
            {
                return NotFound();
            }

            var model =
                new ReservaCreateViewModel
                {
                    IdAlojamiento = id,

                    NombreAlojamiento =
                        alojamiento.Nombre,

                    CapacidadMaxima =
                        alojamiento.CapacidadMaxima,

                    FechaInicio =
                        DateTime.Today,

                    FechaFin =
                        DateTime.Today.AddDays(1),

                    NumeroPersonas = 1
                };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Create(
                ReservaCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var alojamiento =
                    await _alojamientoService
                        .ObtenerPorId(
                            model.IdAlojamiento);

                if (alojamiento == null)
                {
                    return NotFound();
                }

                ValidarReserva(model, alojamiento);

                if (!ModelState.IsValid)
                {
                    model.NombreAlojamiento =
                        alojamiento.Nombre;

                    model.CapacidadMaxima =
                        alojamiento.CapacidadMaxima;

                    return View(model);
                }

                var tarifa =
                    await _service
                        .CalcularTarifa(
                            model.IdAlojamiento,
                            model.NumeroPersonas,
                            model.FechaInicio,
                            model.FechaFin);

                var usuarioId =
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(usuarioId))
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "No se pudo obtener el usuario logueado.");

                    model.NombreAlojamiento =
                        alojamiento.Nombre;

                    model.CapacidadMaxima =
                        alojamiento.CapacidadMaxima;

                    return View(model);
                }

                var reserva = new Reserva
                {
                    IdAlojamiento =
                        model.IdAlojamiento,

                    FechaInicio =
                        model.FechaInicio,

                    FechaFin =
                        model.FechaFin,

                    NumeroPersonas =
                        model.NumeroPersonas,

                    Total =
                        tarifa.TotalPagar,

                    FechaReserva =
                        DateTime.Now,

                    IdUsuario =
                        usuarioId
                };

                await _service
                    .CrearReserva(reserva);

                TempData["success"] =
                    "Reserva creada correctamente.";

                return RedirectToAction(
                    nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(model);
            }
        }

        #endregion

        #region Disponibilidad

        [HttpGet]
        public IActionResult Disponibilidad()
        {
            return View(
                new List<DisponibilidadDto>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Disponibilidad(
                DateTime fechaInicio,
                DateTime fechaFin)
        {
            if (fechaInicio < DateTime.Today)
            {
                TempData["error"] =
                    "La fecha inicio no puede ser menor a hoy.";

                return View(
                    new List<DisponibilidadDto>());
            }

            if (fechaFin <= fechaInicio)
            {
                TempData["error"] =
                    "La fecha fin debe ser mayor a la fecha inicio.";

                return View(
                    new List<DisponibilidadDto>());
            }

            var resultado =
                await _service
                    .ConsultarDisponibilidad(
                        fechaInicio,
                        fechaFin);

            return View(resultado);
        }

        #endregion

        #region Tarifas

        [HttpPost]
        public async Task<JsonResult>
            CalcularTarifa(
                [FromBody]
                CalcularTarifaRequest request)
        {
            try
            {
                var resultado =
                    await _service
                        .CalcularTarifa(
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

        #endregion

        #region Validaciones

        private async void ValidarReserva(
            ReservaCreateViewModel model,
            Alojamiento alojamiento)
        {
            if (model.FechaInicio < DateTime.Today)
            {
                ModelState.AddModelError(
                    "FechaInicio",
                    "La fecha inicio no puede ser menor a hoy.");
            }

            if (model.FechaFin <= model.FechaInicio)
            {
                ModelState.AddModelError(
                    "FechaFin",
                    "La fecha fin debe ser mayor a la fecha inicio.");
            }

            if (model.NumeroPersonas <= 0)
            {
                ModelState.AddModelError(
                    "NumeroPersonas",
                    "Debe ingresar mínimo 1 persona.");
            }

            if (model.NumeroPersonas >
                alojamiento.CapacidadMaxima)
            {
                ModelState.AddModelError(
                    "NumeroPersonas",
                    $"La capacidad máxima es {alojamiento.CapacidadMaxima}");
            }

            var disponibles =
                await _service
                    .ConsultarDisponibilidad(
                        model.FechaInicio,
                        model.FechaFin);

            var alojamientoDisponible =
                disponibles.Any(x =>
                    x.IdAlojamiento ==
                    model.IdAlojamiento);

            if (!alojamientoDisponible)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "El alojamiento no está disponible para esas fechas.");
            }
        }

        #endregion
    }
}