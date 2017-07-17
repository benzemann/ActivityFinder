using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace ActivityModel
{
    public class ActivityManager
    {

        public List<Activity> GetNearActivities(float latitude, float longitude, float distance)
        {
            using (var ctx = new ActivityContext())
            {
                var latParam = new SqlParameter("@latitude", latitude);
                var longParam = new SqlParameter("@longitude", longitude);
                var distanceParam = new SqlParameter("@distance", distance);

                return ctx.Database
                .SqlQuery<Activity>("FindCloseActivities @latitude, @longitude, @distance", latParam, longParam, distanceParam)
                .ToList();
            }
        }

        /// <summary>
        /// Create records for each activity from the given list in the database
        /// </summary>
        /// <param name="activities"></param>
        public void CreateActivitiesInDB(List<Activity> activities)
        {
            using (var ctx = new ActivityContext())
            {
                ctx.Activities.AddRange(activities);
                ctx.SaveChanges();
            }
        }
    }
}
