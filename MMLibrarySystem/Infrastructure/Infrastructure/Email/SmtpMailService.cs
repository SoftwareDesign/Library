using System.Net.Mail;

namespace BookLibrary.Infrastructure.Email
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
            SmtpClient smtpClient = new SmtpClient("VSC-MAIL2010-1.corp.mm-software.com");
            smtpClient.Credentials = new System.Net.NetworkCredential("test@mm-software.com", string.Empty);
            return smtpClient;
        }
    }
}