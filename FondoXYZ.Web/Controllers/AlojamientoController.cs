using FondoXYZ.Application.Common;
using FondoXYZ.Application.Ports.In;
using FondoXYZ.Domain.Exceptions;
using FondoXYZ.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FondoXYZ.Web.Controllers;

[Authorize(Roles = Roles.Admin)]
public class AlojamientoController : Controller
{
    private readonly IAlojamientoService _alojamientoService;
    private readonly ISedeService _sedeService;

    public AlojamientoController(IAlojamientoService alojamientoService, ISedeService sedeService)
    {
        _alojamientoService = alojamientoService;
        _sedeService = sedeService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
        View(await _alojamientoService.ObtenerTodosAsync(cancellationToken));

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await CargarSedesAsync(cancellationToken);
        return View(new AlojamientoFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AlojamientoFormViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _alojamientoService.CrearAsync(model.ToCrearCommand(), cancellationToken);

                TempData["success"] = "Alojamiento creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        await CargarSedesAsync(cancellationToken);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var alojamiento = await _alojamientoService.ObtenerPorIdAsync(id, cancellationToken);

        if (alojamiento is null)
            return NotFound();

        await CargarSedesAsync(cancellationToken);
        return View(AlojamientoFormViewModel.FromDto(alojamiento));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AlojamientoFormViewModel model, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _alojamientoService.ActualizarAsync(model.ToActualizarCommand(), cancellationToken);

                TempData["success"] = "Alojamiento actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (EntidadNoEncontradaException)
            {
                return NotFound();
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        await CargarSedesAsync(cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _alojamientoService.EliminarAsync(id, cancellationToken);
            TempData["success"] = "Alojamiento eliminado correctamente.";
        }
        catch (DomainException ex)
        {
            TempData["error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarSedesAsync(CancellationToken cancellationToken)
    {
        var sedes = await _sedeService.ObtenerTodasAsync(cancellationToken);

        ViewBag.Sedes = new SelectList(sedes, "IdSede", "Nombre");
    }
}
