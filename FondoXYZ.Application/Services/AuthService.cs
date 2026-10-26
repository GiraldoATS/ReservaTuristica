using FondoXYZ.Application.Common;
using FondoXYZ.Application.DTOs;
using FondoXYZ.Application.Ports.In;
using FondoXYZ.Application.Ports.Out;

namespace FondoXYZ.Application.Services;

internal sealed class AuthService : IAuthService
{
    private readonly IIdentityService _identity;
    private readonly INotificacionService _notificaciones;

    public AuthService(IIdentityService identity, INotificacionService notificaciones)
    {
        _identity = identity;
        _notificaciones = notificaciones;
    }

    public Task<bool> LoginAsync(LoginCommand command) =>
        _identity.IniciarSesionAsync(command.Email, command.Password, command.Recordarme);

    public Task<Result> RegistrarAsync(RegistrarUsuarioCommand command) =>
        _identity.RegistrarAsync(command.NombreCompleto, command.Email, command.Password);

    public Task LogoutAsync() => _identity.CerrarSesionAsync();

    public async Task SolicitarRecuperacionPasswordAsync(
        string email,
        string urlRestablecimiento,
        CancellationToken cancellationToken = default)
    {
        var token = await _identity.GenerarTokenRecuperacionAsync(email);

        // Si el correo no existe no se informa nada, para no revelar qué cuentas están registradas.
        if (token is null)
            return;

        var enlace = $"{urlRestablecimiento}?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

        await _notificaciones.EnviarEnlaceRecuperacionPasswordAsync(email, enlace, cancellationToken);
    }

    public Task<Result> RestablecerPasswordAsync(RestablecerPasswordCommand command) =>
        _identity.RestablecerPasswordAsync(command.Email, command.Token, command.NuevaPassword);
}
