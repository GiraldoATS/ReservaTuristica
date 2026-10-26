using System.Text;
using FondoXYZ.Application.Common;
using FondoXYZ.Application.Ports.Out;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace FondoXYZ.Infrastructure.Identity;

/// <summary>
/// Adaptador del puerto de identidad sobre ASP.NET Core Identity.
/// </summary>
internal sealed class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> IniciarSesionAsync(string email, string password, bool recordarme)
    {
        await _signInManager.SignOutAsync();

        var usuario = await _userManager.FindByEmailAsync(email);

        if (usuario is null)
            return false;

        var resultado = await _signInManager.PasswordSignInAsync(
            usuario,
            password,
            recordarme,
            lockoutOnFailure: false);

        return resultado.Succeeded;
    }

    public Task CerrarSesionAsync() => _signInManager.SignOutAsync();

    public async Task<Result> RegistrarAsync(string nombreCompleto, string email, string password)
    {
        if (await _userManager.FindByEmailAsync(email) is not null)
            return Result.Fallo("El correo ya está registrado.");

        var usuario = new ApplicationUser
        {
            UserName = email,
            Email = email,
            NombreCompleto = nombreCompleto.Trim()
        };

        var resultado = await _userManager.CreateAsync(usuario, password);

        return ToResult(resultado);
    }

    public async Task<string?> GenerarTokenRecuperacionAsync(string email)
    {
        var usuario = await _userManager.FindByEmailAsync(email);

        if (usuario is null)
            return null;

        var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);

        // Base64Url hace el token seguro para viajar en una URL sin problemas de codificación.
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
    }

    public async Task<Result> RestablecerPasswordAsync(string email, string token, string nuevaPassword)
    {
        var usuario = await _userManager.FindByEmailAsync(email);

        if (usuario is null)
            return Result.Fallo("El enlace de recuperación no es válido.");

        string tokenDecodificado;

        try
        {
            tokenDecodificado = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        }
        catch (FormatException)
        {
            return Result.Fallo("El enlace de recuperación no es válido.");
        }

        var resultado = await _userManager.ResetPasswordAsync(usuario, tokenDecodificado, nuevaPassword);

        return ToResult(resultado);
    }

    private static Result ToResult(IdentityResult resultado) =>
        resultado.Succeeded
            ? Result.Ok()
            : Result.Fallo(string.Join(" ", resultado.Errors.Select(e => e.Description)));
}
