﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace MMLibrarySystem.Utilities
{
    /// <summary>
    /// Provides utility methods.
    /// </summary>
    internal static class Utility
    {
        public static MailMessage BuildMail(string to, string subject, string body)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;
            return message;
        }
    }
}