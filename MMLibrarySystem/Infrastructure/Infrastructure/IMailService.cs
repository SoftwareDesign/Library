using System.Net.Mail;

namespace MMLibrarySystem.Infrastructure
{
    public interface IMailService
    {
        void Send(MailMessage message);
    }
}