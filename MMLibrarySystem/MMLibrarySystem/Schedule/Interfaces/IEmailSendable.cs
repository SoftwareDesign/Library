namespace MMLibrarySystem.Schedule.Interfaces
{
    public interface IEmailSendable
    {
        void SendEmail(EmailContext emailContext);
    }
}