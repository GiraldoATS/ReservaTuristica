using FondoXYZ.Entities.DTOs;
using FondoXYZ.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FondoXYZ.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(
            IAuthService authService)
        {
            _authService = authService;
        }

        #region Login

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result =
                await _authService.LoginAsync(dto);

            if (!result)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Correo o contraseña incorrectos");

                return View(dto);
            }

            return RedirectToAction(
                "Index",
                "Home");
        }

        #endregion

        #region Register

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result =
                await _authService.RegisterAsync(dto);

            if (!result.Success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.ErrorMessage);

                return View(dto);
            }

            return RedirectToAction(
                "Login");
        }

        #endregion

        #region Logout

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();

            return RedirectToAction(
                "Login");
        }

        #endregion

        #region Password Recovery

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            ForgotPassword(ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _authService
                .ForgotPasswordAsync(dto.Email);

            ViewBag.Message =
                "Si el correo existe se enviará un enlace.";

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(
            string email,
            string token)
        {
            var dto = new ResetPasswordDto
            {
                Email = email,
                Token = token
            };

            return View(dto);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            ResetPassword(ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result =
                await _authService
                    .ResetPasswordAsync(dto);

            if (!result.Success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.ErrorMessage);

                return View(dto);
            }

            return RedirectToAction(
                "Login");
        }

        #endregion

        #region Access Denied

        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion
    }
}