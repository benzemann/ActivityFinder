using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ActivityModel;
using System.Threading.Tasks;


namespace WebService.Controllers
{

    public class ActivityController : ApiController
    {
        /// <summary>
        /// Get activites close to the given address
        /// </summary>
        /// <param name="address">Address, city, or postal number</param>
        /// <returns>Array of close activities</returns>
        public async Task<Activity[]> GetCloseActivities(string address)
        {
            // Get latitude longitude from address
            var latlong = await Helper.LatLongFromAddress(address);

            var mng = new  ActivityManager();
            var activities = mng.GetNearActivities(latlong.Lat, latlong.Long, 5.0f);
            return activities.ToArray();
        }

    }
}
