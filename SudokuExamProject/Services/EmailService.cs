using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Threading.Tasks;

namespace SudokuExamProject.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendContactMessageAsync(
            string name,
            string email,
            string subject,
            string message)
        {
            var mail = new MimeMessage();

            mail.From.Add(
                MailboxAddress.Parse(
                    _configuration["EmailSettings:SenderEmail"]));

            mail.To.Add(
                MailboxAddress.Parse(
                    _configuration["EmailSettings:ReceiverEmail"]));

            mail.Subject = $"Contact mesajı: {subject}";

            mail.Body = new TextPart("plain")
            {
                Text =
                    $"Göndərən: {name}\n" +
                    $"Email: {email}\n" +
                    $"Mövzu: {subject}\n\n" +
                    $"Mesaj:\n{message}"
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                "smtp.gmail.com",
                587,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _configuration["EmailSettings:SenderEmail"],
                _configuration["EmailSettings:AppPassword"]);

            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);
        }
    }
}

