using System.Collections.Generic;
using MMLibrarySystem.Schedule.ScheduleRules;

namespace MMLibrarySystem.Schedule
{
    public class RulesCollection
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