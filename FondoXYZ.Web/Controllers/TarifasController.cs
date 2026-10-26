using FondoXYZ.Application.Common;
using FondoXYZ.Application.Ports.In;
using FondoXYZ.Domain.Exceptions;
using FondoXYZ.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FondoXYZ.Web.Controllers;

[Authorize(Roles = Roles.Admin)]
public class TarifasController : Controller
{
    private readonly ITarifaService _tarifaService;
    private readonly IAlojamientoService _alojamientoService;
    private readonly ITemporadaService _temporadaService;

    public TarifasController(
        ITarifaService tarifaService,
        IAlojamientoService alojamientoService,
        ITemporadaService temporadaService)
    {
        _tarifaService = tarifaService;
        _alojamientoService = alojamientoService;
        _temporadaService = temporadaService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
        View(await _tarifaService.ObtenerTodasAsync(cancellationToken));

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await CargarCombosAsync(cancellationToken);
        return View(new TarifaFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TarifaFormViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _tarifaService.CrearAsync(model.ToCrearCommand(), cancellationToken);

                TempData["success"] = "Tarifa creada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        await CargarCombosAsync(cancellationToken);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var tarifa = await _tarifaService.ObtenerPorIdAsync(id, cancellationToken);

        if (tarifa is null)
            return NotFound();

        await CargarCombosAsync(cancellationToken);
        return View(TarifaFormViewModel.FromDto(tarifa));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TarifaFormViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _tarifaService.ActualizarAsync(model.ToActualizarCommand(), cancellationToken);

                TempData["success"] = "Tarifa actualizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        await CargarCombosAsync(cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _tarifaService.EliminarAsync(id, cancellationToken);
            TempData["success"] = "Tarifa eliminada correctamente";
        }
        catch (DomainException ex)
        {
            TempData["error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarCombosAsync(CancellationToken cancellationToken)
    {
        var alojamientos = await _alojamientoService.ObtenerTodosAsync(cancellationToken);
        var temporadas = await _temporadaService.ObtenerTodasAsync(cancellationToken);

        ViewBag.Alojamientos = new SelectList(alojamientos, "IdAlojamiento", "Nombre");
        ViewBag.Temporadas = new SelectList(temporadas, "IdTemporada", "Nombre");
    }
}
