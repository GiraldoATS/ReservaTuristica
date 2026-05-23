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
        // CREATE GET
        // =========================

        [HttpGet]
        public async Task<IActionResult>
            Create(int id)
        {
            Console.WriteLine(
                "========== CREATE GET ==========");

            Console.WriteLine(
                $"Id recibido: {id}");

            var alojamiento =
                await _alojamientoService
                    .ObtenerPorId(id);

            if (alojamiento == null)
            {
                Console.WriteLine(
                    "ALOJAMIENTO NULL");

                return NotFound();
            }

            Console.WriteLine(
                $"Alojamiento encontrado: {alojamiento.Nombre}");

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

        // =========================
        // CREATE POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Create(
                ReservaCreateViewModel model)
        {
            try
            {
                Console.WriteLine(
                    "========== CREATE POST ==========");

                Console.WriteLine(
                    $"Usuario autenticado?: {User.Identity?.IsAuthenticated}");

                Console.WriteLine(
                    $"Usuario: {User.Identity?.Name}");

                Console.WriteLine(
                    $"IdAlojamiento: {model.IdAlojamiento}");

                Console.WriteLine(
                    $"FechaInicio: {model.FechaInicio}");

                Console.WriteLine(
                    $"FechaFin: {model.FechaFin}");

                Console.WriteLine(
                    $"NumeroPersonas: {model.NumeroPersonas}");

                Console.WriteLine(
                    $"Total: {model.Total}");

                // =========================
                // VALIDAR MODELSTATE
                // =========================

                if (!ModelState.IsValid)
                {
                    Console.WriteLine(
                        "MODELSTATE INVALIDO");

                    foreach (var error in ModelState)
                    {
                        foreach (var subError in error.Value.Errors)
                        {
                            Console.WriteLine(
                                $"CAMPO: {error.Key}");

                            Console.WriteLine(
                                $"ERROR: {subError.ErrorMessage}");
                        }
                    }

                    return View(model);
                }

                // =========================
                // BUSCAR ALOJAMIENTO
                // =========================

                Console.WriteLine(
                    "Buscando alojamiento...");

                var alojamiento =
                    await _alojamientoService
                        .ObtenerPorId(
                            model.IdAlojamiento);

                if (alojamiento == null)
                {
                    Console.WriteLine(
                        "ALOJAMIENTO NULL");

                    return NotFound();
                }

                Console.WriteLine(
                    $"Alojamiento encontrado: {alojamiento.Nombre}");

                // =========================
                // VALIDAR FECHAS
                // =========================

                if (model.FechaInicio
                    < DateTime.Today)
                {
                    ModelState.AddModelError(
                        "FechaInicio",
                        "La fecha inicio no puede ser menor a hoy.");
                }

                if (model.FechaFin
                    <= model.FechaInicio)
                {
                    ModelState.AddModelError(
                        "FechaFin",
                        "La fecha fin debe ser mayor a la fecha inicio.");
                }

                // =========================
                // VALIDAR PERSONAS
                // =========================

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

                // =========================
                // VALIDAR DISPONIBILIDAD
                // =========================

                Console.WriteLine(
                    "Consultando disponibilidad...");

                var disponibles =
                    await _service
                        .ConsultarDisponibilidad(
                            model.FechaInicio,
                            model.FechaFin);

                var alojamientoDisponible =
                    disponibles.Any(x =>
                        x.IdAlojamiento ==
                        model.IdAlojamiento);

                Console.WriteLine(
                    $"Disponible?: {alojamientoDisponible}");

                if (!alojamientoDisponible)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "El alojamiento no está disponible para esas fechas.");
                }

                // =========================
                // VALIDAR MODELSTATE
                // =========================

                if (!ModelState.IsValid)
                {
                    Console.WriteLine(
                        "MODELSTATE INVALIDO DESPUES VALIDACIONES");

                    model.NombreAlojamiento =
                        alojamiento.Nombre;

                    model.CapacidadMaxima =
                        alojamiento.CapacidadMaxima;

                    return View(model);
                }

                // =========================
                // CALCULAR TARIFA
                // =========================

                Console.WriteLine(
                    "Calculando tarifa...");

                var tarifa =
                    await _service
                        .CalcularTarifa(
                            model.IdAlojamiento,
                            model.NumeroPersonas,
                            model.FechaInicio,
                            model.FechaFin);

                Console.WriteLine(
                    $"Total tarifa: {tarifa.TotalPagar}");

                // =========================
                // OBTENER USUARIO LOGUEADO
                // =========================

                var usuarioId =
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier);

                Console.WriteLine(
                    $"Usuario logueado: {usuarioId}");

                if (string.IsNullOrEmpty(usuarioId))
                {
                    Console.WriteLine(
                        "USUARIO NULL");

                    ModelState.AddModelError(
                        string.Empty,
                        "No se pudo obtener el usuario logueado.");

                    model.NombreAlojamiento =
                        alojamiento.Nombre;

                    model.CapacidadMaxima =
                        alojamiento.CapacidadMaxima;

                    return View(model);
                }

                // =========================
                // CREAR RESERVA
                // =========================

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

                Console.WriteLine(
                    "Entidad reserva creada");

                // =========================
                // GUARDAR
                // =========================

                Console.WriteLine(
                    "Guardando reserva...");

                await _service
                    .CrearReserva(reserva);

                Console.WriteLine(
                    "RESERVA GUARDADA OK");

                TempData["success"] =
                    "Reserva creada correctamente.";

                return RedirectToAction(
                    nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "========== ERROR GENERAL ==========");

                Console.WriteLine(
                    ex.Message);

                Console.WriteLine(
                    ex.StackTrace);

                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(model);
            }
        }

        // =========================
        // DISPONIBILIDAD GET
        // =========================

        [HttpGet]
        public IActionResult Disponibilidad()
        {
            return View(
                new List<DisponibilidadDto>());
        }

        // =========================
        // DISPONIBILIDAD POST
        // =========================

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

        // =========================
        // CALCULAR TARIFA AJAX
        // =========================

        [HttpPost]
        public async Task<JsonResult>
            CalcularTarifa(
                [FromBody]
                CalcularTarifaRequest request)
        {
            try
            {
                Console.WriteLine(
                    "========== CALCULAR TARIFA ==========");

                Console.WriteLine(
                    $"IdAlojamiento: {request.IdAlojamiento}");

                Console.WriteLine(
                    $"NumeroPersonas: {request.NumeroPersonas}");

                var resultado =
                    await _service
                        .CalcularTarifa(
                            request.IdAlojamiento,
                            request.NumeroPersonas,
                            request.FechaInicio,
                            request.FechaFin);

                Console.WriteLine(
                    $"Total calculado: {resultado.TotalPagar}");

                return Json(resultado);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "ERROR CALCULAR TARIFA");

                Console.WriteLine(
                    ex.Message);

                return Json(new
                {
                    error = ex.Message
                });
            }
        }
    }
}