using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using BookLibrary.Schedule;

namespace BookLibrary
{
    public partial class ScheduleService : ServiceBase
    {
        private DailyPlan _dailyPlan;

        public ScheduleService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _dailyPlan = new DailyPlan();
        }

        protected override void OnStop()
        {
            _dailyPlan.Dispose();
        }
    }
}
