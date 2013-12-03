using System.Net.Mail;

namespace BookLibrary.Infrastructure
{
    public interface IMailService
    {
        void Send(MailMessage message);
    }
}