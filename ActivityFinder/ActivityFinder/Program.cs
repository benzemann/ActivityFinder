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
            // Create activity manager
            var mng = new ActivityManager();
            // List of to store all activites from the different sites
            var activities = new List<Activity>();
            // Get Activities from ticket master
            var ticketMasterActivities = new List<Activity>();
            //TicketMasterAPI.GetAllActivities(ticketMasterActivities).Wait();
            log.Debug($"Found {ticketMasterActivities.Count} activites from Ticketmaster");
            // Get Activities from google maps
            var googleMapsActivities = new List<Activity>();
            //GoogleMaps.GoogleMapsAPI.GetAllActivities(googleMapsActivities).Wait();
            log.Debug($"Found {googleMapsActivities.Count} activities from googleMaps");
            // Get Activities from tripadvisor
            var tripAdvisorActivities = new List<Activity>();
            //TripAdvisor.TripAdvisorAPI.GetAllActivities(tripAdvisorActivities);
            log.Debug($"Found {tripAdvisorActivities.Count} activities from tripadvisor");
            var visitDenmarkActivities = new List<Activity>();
            VisitDenmark.VisitDenmarkAPI.GetAllActivities(visitDenmarkActivities);
            log.Debug($"Found {visitDenmarkActivities.Count} activities from visitdenmark");
            // Add all activities to the database
            activities.AddRange(ticketMasterActivities);
            activities.AddRange(googleMapsActivities);
            activities.AddRange(tripAdvisorActivities);
            activities.AddRange(visitDenmarkActivities);
            log.Debug($"Total number of activities found: {activities.Count}");
            SetMapImagesOnUnsetImages(activities);
            log.Debug("Delete all activities in the database");
            mng.DeleteAllActivities();
            log.Debug("CreateActivitiesInDB: Creating activities");
            mng.AddActivitiesToDB(activities);
            log.Debug("CreateActivitiesInDB: Complete");
            log.Debug("Ended");
            Console.ReadLine();
        }

        static void SetMapImagesOnUnsetImages(List<Activity> activities)
        {
            log.Debug("SetMapImagesOnUnsetImages: Started");
            for(int i = 0; i < activities.Count; i++)
            {
                if(activities[i].Image == null)
                {
                    activities[i].Image = GoogleMaps.GoogleMapsAPI.GetStaticMapUrl(
                        activities[i].Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                        "," +
                        activities[i].Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)
                        );
                }
            }
            log.Debug("SetMapImagesOnUnsetImages: Ended");
        }
    }
}
