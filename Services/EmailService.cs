using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace SecureVault.Services
{
    public class EmailService
    {
        private IConfiguration configuration;

        public EmailService(IConfiguration configuration) 
        {
            this.configuration = configuration;
        }

        public void SendEmail(string email, string subject, string htmlMessage)
        {
            string? fromMail = configuration.GetValue<string>("EmailLogin:EMAILID");
            string? fromPassword = configuration.GetValue<string>("EmailLogin:APPPASSWORD");

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };
            smtpClient.Send(message);
        }

        public string GenerateEmailConfirmationToken()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            byte[] tokenData = new byte[32];
            rng.GetBytes(tokenData);

            return Convert.ToBase64String(tokenData);
        }

        public string GeneratePasswordResetToken()
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            byte[] tokenData = new byte[64];
            rng.GetBytes(tokenData);

            return Convert.ToBase64String(tokenData);
        }
    }
}
