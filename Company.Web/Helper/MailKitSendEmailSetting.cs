using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Company.Web.Helper
{
    public class MailKitSendEmailSetting(IOptions<MailSetting> _options) : IEmailServices
    {
        
        public void SendEmail(Email email)
        {
            // 1. Build Message 
            var mail = new MimeMessage();
            mail.Subject = email.Subject;
            mail.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();

            // 2. Stablish Connection
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Value.Host, _options.Value.Port,MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.Email, _options.Value.Password);

           
        }
    }
}
