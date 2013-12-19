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
    public partial class ScheduleService : ServiceBase, ILogger
    {
        private const string ScheduleLogSource = "BookLibSchedule";

        private const string ScheduleLogName = "BookLibScheduleLog";

        private DailyPlan _dailyPlan;

        public ScheduleService()
        {
            InitializeComponent();
            CanStop = true;
            CanShutdown = true;
            CanPauseAndContinue = true;

            if (!EventLog.SourceExists(ScheduleLogSource))
            {
                EventLog.CreateEventSource(ScheduleLogSource, ScheduleLogName);
            }

            _eventLog.Source = ScheduleLogSource;
            _eventLog.Log = ScheduleLogName;
        }

        void ILogger.Log(string format, params object[] args)
        {
            LogEvent(format, args);
        }

        protected override void OnStart(string[] args)
        {
            LogEvent("On Service Start");

            _dailyPlan = new DailyPlan(this);
        }

        protected override void OnStop()
        {
            LogEvent("On Service Stop");

            _dailyPlan.Dispose();
        }

        protected override void OnShutdown()
        {
            LogEvent("On System Shutdown");

            _dailyPlan.Dispose();
        }

        protected override void OnPause()
        {
            LogEvent("On Service Pause");

            _dailyPlan.Pause();
        }

        protected override void OnContinue()
        {
            LogEvent("On Service Continue");

            _dailyPlan.Continue();
        }

        private void LogEvent(string format, params object[] args)
        {
            var message = string.Format(format, args);
            _eventLog.WriteEntry(message);
        }
    }
}
