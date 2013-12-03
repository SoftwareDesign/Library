using System.Collections.Generic;
using BookLibrary.Schedule.ScheduleRules;

namespace BookLibrary.Schedule
{
    internal class RulesCollection
    {
        public static List<IBorrowCheckRule> GetRules()
        {
            var schedulaList = new List<IBorrowCheckRule>();
            schedulaList.Add(new OutOfDateRule());
            schedulaList.Add(new AheadNotificationRule());
            return schedulaList;
        }
    }
}