using System.Net.Mail;
using MMLibrarySystem.Schedule.Interfaces;

namespace MMLibrarySystem.Schedule.EmaiImplementations
{
    public class EmailSenderBySmtp : IEmailSendable
    {
        public void SendEmail(EmailContext emailContext)
        {
            string adress = emailContext.Adress;
            string subject = emailContext.Subject;
            string body = emailContext.Body;
            SmtpClient smtpClient = new SmtpClient("mail.mm-software.com", 25);
            smtpClient.Credentials = new System.Net.NetworkCredential("test@mm-software.com", "myIDPassword");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress("test@mm-software.com", "MMLibrary");
            mail.To.Add(new MailAddress(adress));
            smtpClient.Send(mail);
        }
    }
}