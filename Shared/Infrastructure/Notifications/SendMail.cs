using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Shared.Infrastructure.Notications
{
    public class EnviadorDeCorreos(IConfiguration configuration)
    {
        private readonly string mailFrom = configuration["Correo:MailFrom"] ?? throw new ArgumentNullException(nameof(configuration));
        private readonly string password = configuration["Correo:Password"] ?? throw new ArgumentNullException(nameof(configuration));
        private readonly string smtpClient = configuration["Correo:SmtpClient"] ?? throw new ArgumentNullException(nameof(configuration));

        public bool SendMail(string correoDestino, string subject, string body)
        {
            try
            {
                var mail = new MailMessage(mailFrom, correoDestino)
                {
                    Subject = subject,
                    Body = body
                };

                var smtpClientMailer = new SmtpClient(smtpClient, 587)
                {
                    Credentials = new NetworkCredential(mailFrom, password),
                    EnableSsl = true
                };

                smtpClientMailer.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                using (StreamWriter sw = new("MailerExceptionLog.txt", true))
                {
                    sw.WriteLine($"MailerException: {e.Message}");
                }
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }
    }
}
