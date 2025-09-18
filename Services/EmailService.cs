using FastFood.Services.Interfaces;
using NuGet.Packaging;
using System.Net.Mail;

namespace FastFood.Services
{
    public class EmailService : CommonService, IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string receivers, string subject, string body)
        {
            string sender = this._configuration["MailServer:Gmail:Sender"] ?? string.Empty;
            string secret = this._configuration["MailServer:Gmail:Secret"] ?? string.Empty;
            string host = this._configuration["MailServer:Gmail:Host"] ?? string.Empty;
            int port = Convert.ToInt32(this._configuration["MailServer:Gmail:Port"] ?? string.Empty);

            using (MailMessage mail = new MailMessage())
            {

                mail.To.Add(receivers);
                mail.From = new MailAddress(sender);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(sender, secret),
                    EnableSsl = true
                };

                await smtp.SendMailAsync(mail);
            }
        }
    }
}
