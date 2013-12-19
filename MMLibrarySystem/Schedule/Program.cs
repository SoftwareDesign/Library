using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using BookLibrary.Service;

namespace BookLibrary
{
    static class Program
    {
        /// <summary>
        /// Entry point of the service program.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new ScheduleService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
