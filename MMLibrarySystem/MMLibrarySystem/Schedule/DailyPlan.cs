using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using MMLibrarySystem.Models;

namespace MMLibrarySystem.Schedule
{
    public class DailyPlan
    {
        private Timer _timer;

        private const int _daysBeforeDeadline = 3;

        public DailyPlan()
        {
            _timer=new Timer();
            _timer.Interval = 24*60*60*1000;
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
                var shouldReturnBookRecords = borrowRecords.Where(br => (DateTime.Now - br.BorrowedDate).Days > 31).ToList();
                SendEmail(shouldReturnBookRecords);
            }
        }

        private void SendEmail(List<BorrowRecord> borrowRecords)
        {
        }
    }
}