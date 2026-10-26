using System.Security.Claims;
using FondoXYZ.Application.Ports.In;
using FondoXYZ.Domain.Exceptions;
using FondoXYZ.Web.Models.Reservas;
using Microsoft.AspNetCore.Mvc;

namespace FondoXYZ.Web.Controllers;

public class ReservasController : Controller
{
    private readonly IReservaService _reservaService;
    private readonly IAlojamientoService _alojamientoService;

    public ReservasController(IReservaService reservaService, IAlojamientoService alojamientoService)
    {
        _reservaService = reservaService;
        _alojamientoService = alojamientoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
        View(await _reservaService.ObtenerTodasAsync(cancellationToken));

    [HttpGet]
    public async Task<IActionResult> Create(int id, CancellationToken cancellationToken)
    {
        var model = new ReservaCreateViewModel
        {
            IdAlojamiento = id,
            FechaInicio = DateTime.Today,
            FechaFin = DateTime.Today.AddDays(1),
            NumeroPersonas = 1
        };

        if (!await CargarDatosAlojamientoAsync(model, cancellationToken))
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservaCreateViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var idUsuario = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(idUsuario))
                    return Challenge();

                await _reservaService.CrearAsync(model.ToCommand(idUsuario), cancellationToken);

                TempData["success"] = "Reserva creada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        if (!await CargarDatosAlojamientoAsync(model, cancellationToken))
            return NotFound();

        return View(model);
    }

    [HttpGet]
    public IActionResult Disponibilidad() => View(new DisponibilidadViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Disponibilidad(DisponibilidadViewModel model, CancellationToken cancellationToken)
    {
        if (model.FechaInicio is null || model.FechaFin is null)
        {
            ModelState.AddModelError(string.Empty, "Debe seleccionar la fecha inicio y la fecha fin.");
            return View(model);
        }

        try
        {
            model.Resultados = await _reservaService.ConsultarDisponibilidadAsync(
                model.FechaInicio.Value,
                model.FechaFin.Value,
                cancellationToken);

            model.Consultado = true;
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CalcularTarifa([FromBody] CalcularTarifaRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _reservaService.CalcularTarifaAsync(
                request.IdAlojamiento,
                request.NumeroPersonas,
                request.FechaInicio,
                request.FechaFin,
                cancellationToken);

            return Ok(resultado);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private async Task<bool> CargarDatosAlojamientoAsync(ReservaCreateViewModel model, CancellationToken cancellationToken)
    {
        var alojamiento = await _alojamientoService.ObtenerPorIdAsync(model.IdAlojamiento, cancellationToken);

        if (alojamiento is null)
            return false;

        model.NombreAlojamiento = alojamiento.Nombre;
        model.CapacidadMaxima = alojamiento.CapacidadMaxima;

        return true;
    }
}
