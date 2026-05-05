using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using NRAdmanWebApplicationNet10.Settings;

namespace NRAdmanWebApplicationNet10.Services
{
    

    public class MailService(IOptions<MailSettings> mailSettings) : IEmailSender
    {
        private readonly MailSettings _mailSettings = mailSettings.Value;
        

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                using var body = new TextPart(TextFormat.Html);
                body.Text = htmlMessage;

                var message = new MimeMessage();
                message.To.Add(new MailboxAddress(email, email));
                message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
                message.Subject = subject;
                message.Body = body;
                
                
                using var smtp = new SmtpClient();
                smtp.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private static bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }
    }
}
