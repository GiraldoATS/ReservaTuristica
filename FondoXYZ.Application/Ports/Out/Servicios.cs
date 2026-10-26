using FondoXYZ.Application.Common;

namespace FondoXYZ.Application.Ports.Out;

/// <summary>
/// Abstrae el proveedor de identidad (hoy ASP.NET Core Identity),
/// para que la aplicación no dependa de UserManager/SignInManager.
/// </summary>
public interface IIdentityService
{
    Task<bool> IniciarSesionAsync(string email, string password, bool recordarme);

    Task CerrarSesionAsync();

    Task<Result> RegistrarAsync(string nombreCompleto, string email, string password);

    /// <returns>El token de recuperación, o null si el usuario no existe.</returns>
    Task<string?> GenerarTokenRecuperacionAsync(string email);

    Task<Result> RestablecerPasswordAsync(string email, string token, string nuevaPassword);
}

public interface INotificacionService
{
    Task EnviarEnlaceRecuperacionPasswordAsync(
        string email,
        string enlace,
        CancellationToken cancellationToken = default);
}
