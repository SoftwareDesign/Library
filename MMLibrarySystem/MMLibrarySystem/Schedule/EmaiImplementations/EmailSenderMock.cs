using System;
using MMLibrarySystem.Schedule.Interfaces;
using MMLibrarySystem.Utilities;

namespace MMLibrarySystem.Schedule.EmaiImplementations
{
    public class EmailSenderMock : IEmailSendable
    {
        public void SendEmail(EmailContext context)
        {
            string adress = context.Adress;
            string subject = context.Subject;
            string body = context.Body;
            LogWriter.Write(string.Format("EmailAdress : {0}", adress));
            LogWriter.Write(string.Format("EmailSubject : {0}", subject));
            LogWriter.Write(string.Format("EmailBody : {0}", body));
            LogWriter.Write(Environment.NewLine);
        }
    }
}