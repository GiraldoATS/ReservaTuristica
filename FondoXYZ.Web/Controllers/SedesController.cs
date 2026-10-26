using FondoXYZ.Application.Common;
using FondoXYZ.Application.Ports.In;
using FondoXYZ.Domain.Exceptions;
using FondoXYZ.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FondoXYZ.Web.Controllers;

[Authorize(Roles = Roles.Admin)]
public class SedesController : Controller
{
    private readonly ISedeService _sedeService;

    public SedesController(ISedeService sedeService)
    {
        _sedeService = sedeService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
        View(await _sedeService.ObtenerTodasAsync(cancellationToken));

    [HttpGet]
    public IActionResult Create() => View(new SedeFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SedeFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _sedeService.CrearAsync(model.ToCommand(), cancellationToken);
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }

        TempData["success"] = "Sede creada correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
