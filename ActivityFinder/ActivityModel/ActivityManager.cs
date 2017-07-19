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
        /// <summary>
        /// Get activities in the distance from the given latlong coordinates
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
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
        public void AddActivitiesToDB(List<Activity> activities)
        {
            using (var ctx = new ActivityContext())
            {
                ctx.Activities.AddRange(activities);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes all activities in the database
        /// </summary>
        public void DeleteAllActivities()
        {
            using (var ctx = new ActivityContext())
            {
                ctx.Activities.RemoveRange(ctx.Activities);
                ctx.SaveChanges();
            }
        }
    }
}
