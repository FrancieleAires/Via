
using System.Net;
using System.Net.Mail;

namespace ViaAPI.Services.EmailService
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient(_configuration["ElasticEmail:SmtpServer"])
            {
                Port = int.Parse(_configuration["ElasticEmail:SmtpPort"]),
                Credentials = new NetworkCredential(_configuration["ElasticEmail:Username"], _configuration["ElasticEmail:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["ElasticEmail:FromEmail"], "Equipe Legacy"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
