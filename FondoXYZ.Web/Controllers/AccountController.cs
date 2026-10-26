using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Ports.In;
using FondoXYZ.Web.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FondoXYZ.Web.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var exitoso = await _authService.LoginAsync(
            new LoginCommand(model.Email, model.Password, model.RememberMe));

        if (!exitoso)
        {
            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos");
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var resultado = await _authService.RegistrarAsync(
            new RegistrarUsuarioCommand(model.NombreCompleto, model.Email, model.Password));

        if (!resultado.Exitoso)
        {
            ModelState.AddModelError(string.Empty, resultado.Error);
            return View(model);
        }

        TempData["success"] = "Registro exitoso. Ya puedes iniciar sesión.";
        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult ForgotPassword() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        // La URL se arma con el host actual, en lugar de quemar "https://localhost:7155".
        var urlRestablecimiento = Url.Action(nameof(ResetPassword), "Account", values: null, protocol: Request.Scheme)!;

        await _authService.SolicitarRecuperacionPasswordAsync(model.Email, urlRestablecimiento, cancellationToken);

        ViewBag.Message = "Si el correo existe se enviará un enlace.";
        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string email, string token) =>
        View(new ResetPasswordViewModel { Email = email, Token = token });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var resultado = await _authService.RestablecerPasswordAsync(
            new RestablecerPasswordCommand(model.Email, model.Token, model.Password));

        if (!resultado.Exitoso)
        {
            ModelState.AddModelError(string.Empty, resultado.Error);
            return View(model);
        }

        TempData["success"] = "Contraseña actualizada. Ya puedes iniciar sesión.";
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied() => View();
}
