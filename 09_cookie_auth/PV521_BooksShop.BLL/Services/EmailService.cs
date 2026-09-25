using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PV521_BooksShop.BLL.Settings;
using PV521_BooksShop.DAL.Entities;
using System.Net;
using System.Net.Mail;

namespace PV521_BooksShop.BLL.Services
{
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly SmtpClient _smtpClient;

        public EmailService(IOptions<SmtpSettings> options)
        {
            _smtpSettings = options.Value;

            _smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port);
            _smtpClient.Credentials = new NetworkCredential(_smtpSettings.Email, _smtpSettings.Password);
            _smtpClient.EnableSsl = true;
        }

        public async Task SendEmailConfirmMessageAsync(User user, string token, string callbackUrl, CancellationToken ct = default)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "templates", "confirmEmail.html");
            string url = $"{callbackUrl}?uid={user.Id}&token={token}";
            
            string html = await File.ReadAllTextAsync(path, ct);
            html = html.Replace("{{CONFIRMATION_LINK}}", url);

            var message = new MailMessage();
            message.From = new MailAddress(_smtpSettings.Email);
            message.To.Add(user.Email);
            message.Subject = "Підтвердження пошти";
            message.IsBodyHtml = true;
            message.Body = html;

            await _smtpClient.SendMailAsync(message, ct);
        }
    }
}
