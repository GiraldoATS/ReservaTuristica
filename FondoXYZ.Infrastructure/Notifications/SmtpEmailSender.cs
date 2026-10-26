using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace FondoXYZ.Infrastructure.Notifications;

/// <summary>
/// Responsabilidad única: entregar un correo HTML por SMTP.
/// </summary>
internal sealed class SmtpEmailSender
{
    private readonly EmailSettings _settings;

    public SmtpEmailSender(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml, CancellationToken cancellationToken)
    {
        using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.Username, _settings.Password),
            EnableSsl = true
        };

        using var mensaje = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = asunto,
            Body = cuerpoHtml,
            IsBodyHtml = true
        };

        mensaje.To.Add(destinatario);

        await client.SendMailAsync(mensaje, cancellationToken);
    }
}
