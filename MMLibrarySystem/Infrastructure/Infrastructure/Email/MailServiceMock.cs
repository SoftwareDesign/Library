using System;
using System.Text;
using System.Net.Mail;
using MMLibrarySystem.Utilities;

namespace MMLibrarySystem.Infrastructure.Email
{
    public class MailServiceMock : IMailService
    {
        public void Send(MailMessage message)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Send Email");
            sb.AppendFormat("To : {0}\r\n", message.To);
            sb.AppendFormat("Subject : {0}\r\n", message.Subject);
            sb.AppendFormat("From : {0}\r\n", message.From);
            sb.AppendFormat("Content : {0}\r\n", message.Body);
            sb.AppendLine();
            LogWriter.Write(sb.ToString());
        }
    }
}