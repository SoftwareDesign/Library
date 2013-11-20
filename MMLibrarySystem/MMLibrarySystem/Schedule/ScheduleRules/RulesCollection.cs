using System.Collections.Generic;
using MMLibrarySystem.Schedule.Interfaces;

namespace MMLibrarySystem.Schedule.ScheduleRules
{
    public class RulesCollection
    {
        public static List<ISchedulable> GetRules()
        {
            var schedulaList = new List<ISchedulable>();
            schedulaList.Add(new OutOfDateRule());
            schedulaList.Add(new AheadNotificationRule());
            return schedulaList;
        }
    }
}