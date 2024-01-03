using _15_11_23.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;

namespace _15_11_23.Services
{
    public class EmailService :IEmailService
    {
        private readonly IConfiguration _conf;

        public EmailService(IConfiguration conf)
        {
            _conf = conf;
        }

        public async Task SendMailAsync(string emailTo,string subject, string body, bool isHtml = false)
        {
            SmtpClient smtpClient = new SmtpClient(_conf["Email:Host"], Convert.ToInt32(_conf["Email:Port"]));

            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_conf["Email:LoginEmail"], _conf["Email:Password"]);

            MailAddress from = new MailAddress(_conf["Email:LoginEmail"], "Pronia Administration");
            MailAddress to = new MailAddress(emailTo);

            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isHtml;

            await smtpClient.SendMailAsync(message);
        }
    }
}
