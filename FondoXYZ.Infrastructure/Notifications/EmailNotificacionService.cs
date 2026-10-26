using System.Net;
using FondoXYZ.Application.Ports.Out;

namespace FondoXYZ.Infrastructure.Notifications;

/// <summary>
/// Adaptador del puerto de notificaciones por correo electrónico.
/// </summary>
internal sealed class EmailNotificacionService : INotificacionService
{
    private readonly SmtpEmailSender _emailSender;
    private readonly TimeProvider _timeProvider;

    public EmailNotificacionService(SmtpEmailSender emailSender, TimeProvider timeProvider)
    {
        _emailSender = emailSender;
        _timeProvider = timeProvider;
    }

    public Task EnviarEnlaceRecuperacionPasswordAsync(
        string email,
        string enlace,
        CancellationToken cancellationToken = default)
    {
        var cuerpo = PlantillasCorreo.RecuperacionPassword(
            WebUtility.HtmlEncode(enlace),
            _timeProvider.GetLocalNow().Year);

        return _emailSender.EnviarAsync(email, "Recuperación de contraseña", cuerpo, cancellationToken);
    }
}
