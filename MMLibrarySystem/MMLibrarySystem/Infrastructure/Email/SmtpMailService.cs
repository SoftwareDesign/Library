using System.Net.Mail;

namespace MMLibrarySystem.Infrastructure.Email
{
    public class SmtpMailService : IMailService
    {
        private SmtpClient _smtpClient;

        public SmtpMailService()
        {
            _smtpClient = CreateSmtpClient();
        }

        public void Send(MailMessage message)
        {
            _smtpClient.Send(message);
        }

        private SmtpClient CreateSmtpClient()
        {
            SmtpClient smtpClient = new SmtpClient("mail.mm-software.com", 25);
            smtpClient.Credentials = new System.Net.NetworkCredential("test@mm-software.com", "myIDPassword");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            return smtpClient;
        }
    }
}