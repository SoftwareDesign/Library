using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using MMLibrarySystem.Models;
using Outlook = Microsoft.Office.Interop.Outlook;
namespace MMLibrarySystem.Schedule
{
    public class DailyPlan
    {
        private Timer _timer;

        private const int _daysBeforeDeadline = 3;

        public DailyPlan()
        {
            _timer = new Timer();
            _timer.Interval = 10 * 1000;
            _timer.Elapsed += BeginCheck;
            _timer.Start();
        }

        public void BeginCheck(object sender, ElapsedEventArgs e)
        {
            GetShouldReturnInfoes();
        }

        private void GetShouldReturnInfoes()
        {
            using (BookLibraryContext db = new BookLibraryContext())
            {
                var borrowRecords = db.BorrowRecords;
                var shouldReturnBookRecords = borrowRecords.Where(br => (DateTime.Now - br.BorrowedDate).Days < 31);
                foreach (var shouldReturnBookRecord in shouldReturnBookRecords)
                {

                }
                SendEmail(null);
            }
        }

        private void SendEmail(List<BorrowRecord> borrowRecords)
        {
            var app = new Outlook.Application();
            Outlook.MailItem oMsg = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
            Outlook.Recipient oRecip = oMsg.Recipients.Add("106287961@qq.com");
            oRecip.Resolve();
            oMsg.Subject = "This is the subject of the test message";
            oMsg.Body = "This is the text in the message.";
            oMsg.Save();
            oMsg.Send();
            oRecip = null;
            oMsg = null;
            app = null;
        }
    }
}