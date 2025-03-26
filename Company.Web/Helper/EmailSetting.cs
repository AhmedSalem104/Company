using System.Net;
using System.Net.Mail;

namespace Company.Web.Helper
{
    public class EmailSetting
    {


        // Use Outlook
        public static bool SendEmail(Email email)
        {
            try
            {
                MailMessage mail = new MailMessage();

                // Set the sender's email address (Outlook)
               // mail.From = new MailAddress("42016006@hti.edu.eg");
                mail.From = new MailAddress("42016006@hti.edu.eg", "Company");


                // Set the recipient's email address (Gmail)
                mail.To.Add($"{email.To}");

                // Set the email subject and body

                mail.IsBodyHtml = true;
                // Create a new SmtpClient and configure it for Outlook's SMTP server
                SmtpClient smtpClient = new SmtpClient("smtp.outlook.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("42016006@hti.edu.eg", "ymwgzlhbcsbknqbr");
                smtpClient.EnableSsl = true;

                mail.Body = @" <div style='text-align: center; font-family: Arial, sans-serif; padding: 20px;'>
                                    <p style='color: #555; font-size: 16px;'>Click the link below to reset your password:</p>
                                    <a href='" + email.Body + @"' 
                                       style='color: #007bff; font-size: 18px; text-decoration: none; word-break: break-all;'>
                                       " + email.Body + @"
                                    </a>
                                    <p style='color: #777; font-size: 14px; margin-top: 10px;'>If you did not request this, please ignore this email.</p>
                                </div>";

                mail.Subject = " Reset Password ";
                mail.IsBodyHtml = true;

                // Send the email
                smtpClient.Send(mail);

                // Dispose of the SmtpClient and MailMessage
                smtpClient.Dispose();
                mail.Dispose();


                return true;
            }
            catch(Exception e) { return false; }
          
        }
    }
}
