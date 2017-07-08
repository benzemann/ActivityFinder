using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ActivityModel;
using log4net;

namespace ActivityFinder
{
    class Program
    {

        private static readonly ILog log = log4net.LogManager.GetLogger("Logger");

        static void Main(string[] args)
        {
            log.Debug("Starting ActivityFinder");

            var activities = new List<Activity>();

            log.Debug("Ending ActivityFinder");
            Console.ReadLine();
        }
    }
}
