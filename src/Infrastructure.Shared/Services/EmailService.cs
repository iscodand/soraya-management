using System.Net;
using System.Net.Mail;
using Application.Contracts;
using Application.DTOs.Email;

namespace Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        public EmailService()
        { }

        public async Task SendMailAsync(SendMailRequest request)
        {
            MailMessage mailMessage = new();
            SmtpClient smtp = new();

            string email = Environment.GetEnvironmentVariable("EMAIL_ADDRESS");
            string password = Environment.GetEnvironmentVariable("EMAIL_KEY");
            string host = Environment.GetEnvironmentVariable("EMAIL_HOST");

            _ = int.TryParse(Environment.GetEnvironmentVariable("EMAIL_PORT"),
                             out int port);

            // SMTP Settings
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(email, password);
            smtp.Port = port;
            smtp.EnableSsl = true;
            smtp.Host = host;

            string emailBody = request.Body;

            // se TemplatePath não for nulo, significa que o e-mail possui um
            // template HTML que precisa ser lido pelo Reader.
            if (request.TemplatePath is not null)
            {
                string pathToHtmlFile = $"wwwroot/emailTemplates/{request.TemplatePath}";
                using StreamReader reader = new(pathToHtmlFile);
                emailBody = reader.ReadToEnd();

                foreach (KeyValuePair<string, string> keyValuePair in request.Parameters)
                {
                    emailBody = emailBody.Replace(keyValuePair.Key, keyValuePair.Value);
                }

                mailMessage.IsBodyHtml = true;
            }

            // Create Message
            mailMessage.From = new MailAddress(email);
            mailMessage.To.Add(request.To);
            mailMessage.Subject = request.Subject;
            mailMessage.Body = emailBody;

            // SendMail
            await smtp.SendMailAsync(mailMessage);
        }
    }
}
