using FondoXYZ.Entities.Configurations;
using FondoXYZ.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FondoXYZ.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(
            IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendAsync(
            string to,
            string subject,
            string body)
        {
            using var client = new SmtpClient(
                _settings.SmtpServer,
                _settings.Port);

            client.Credentials =
                new NetworkCredential(
                    _settings.Username,
                    _settings.Password);

            client.EnableSsl = true;

            var mail = new MailMessage
            {
                From = new MailAddress(
                    _settings.SenderEmail,
                    _settings.SenderName),

                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            await client.SendMailAsync(mail);
        }
    }
}
