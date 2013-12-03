using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace BookLibrary.Utilities
{
    /// <summary>
    /// Provides utility methods.
    /// </summary>
    public static class Utility
    {
        public static MailMessage BuildMail(string to, string subject, string body)
        {
            var notifier = GlobalConfigReader.ReadFromGlobalConfig("NotifierEmail", "value");
            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(notifier);
            message.Subject = subject;
            message.Body = body;
            return message;
        }
        
        public static string BuildAlert(string format, params object[] args)
        {
            var sb = new StringBuilder();
            sb.Append("alert('");
            sb.AppendFormat(format, args);
            sb.Append("');");
            return sb.ToString();
        }
        
        public static string BuildConfirm(string format, params object[] args)
        {
            var sb = new StringBuilder();
            sb.Append(" return confirm('");
            sb.AppendFormat(format, args);
            sb.Append("'); ");
            return sb.ToString();
        }
    }
}