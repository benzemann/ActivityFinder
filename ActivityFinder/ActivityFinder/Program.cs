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

        private static readonly ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            log.Debug("Started");
            // List of to store all activites from the different sites
            var activities = new List<Activity>();
            // Get Activities from ticket master
            var ticketMasterActivities = new List<Activity>();
            TicketMasterAPI.GetAllActivities(ticketMasterActivities).Wait();
            log.Debug($"Found {ticketMasterActivities.Count} activites from Ticketmaster");
            // Get Activities from google maps
            var googleMapsActivities = new List<Activity>();
            GoogleMaps.GoogleMapsAPI.GetAllActivities(googleMapsActivities).Wait();
            log.Debug($"Found {googleMapsActivities.Count} activities from googleMaps");
            // Add all activities to the database
            activities.AddRange(ticketMasterActivities);
            activities.AddRange(googleMapsActivities);
            log.Debug($"Total number of activities found: {activities.Count}");
            log.Debug("CreateActivitiesInDB: Creating activities");
            var mng = new ActivityManager();
            mng.CreateActivitiesInDB(activities);
            log.Debug("CreateActivitiesInDB: Complete");
            log.Debug("Ended");
            Console.ReadLine();
        }
    }
}
