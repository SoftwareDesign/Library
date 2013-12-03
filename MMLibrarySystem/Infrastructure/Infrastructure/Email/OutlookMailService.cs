using System.Net.Mail;
//using Outlook = Microsoft.Office.Interop.Outlook;

namespace BookLibrary.Infrastructure.Email
{
    public class EmailSenderByOutlook : IMailService
    {
        public void Send(MailMessage message)
        {
            //var app = new Outlook.Application();
            //Outlook.MailItem oMsg = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
            //Outlook.Recipient oRecip = oMsg.Recipients.Add("106287961@qq.com");
            //oRecip.Resolve();
            //oMsg.Subject = "This is the subject of the test message";
            //oMsg.Body = "This is the text in the message.";
            //oMsg.Save();
            //oMsg.Send();
            //oRecip = null;
            //oMsg = null;
            //app = null;
        }
    }
}