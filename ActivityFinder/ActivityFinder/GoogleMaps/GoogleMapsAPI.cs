using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Configuration;
using System.Threading.Tasks;
using log4net;
using ActivityModel;

namespace ActivityFinder.GoogleMaps
{
    static class GoogleMapsAPI
    {

        private static readonly ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string[] types = new string[] { "bowling_alley", "amusement_park", "aquarium", "art_gallery",
        "casino", "movie_theater", "museum", "spa", "zoo" };
        private static string[] latlongs = new string[] { "55.67594,12.56553", "57.048,9.9187", "56.15674,10.21076",
        "55.67938,12.53463", "55.39594,10.38831", "55.25377,9.48982" }; // mayor cities in DK

        public async static Task GetAllActivities(List<Activity> activities)
        {
            log.Debug($"GetAllActivities: Start iterating all latlongs and types");

            foreach (var type in types)
            {
                foreach(var latlong in latlongs)
                {
                    await GetAllActivities(activities, latlong, type);
                }
            }
            // Remove duplicates by place_id and remove it from the description field
            activities = activities.AsEnumerable().GroupBy(x => x.Description).Select(x => x.First()).ToList();
            foreach(var a in activities)
            {
                a.Description = "";
            }
            
            log.Debug($"GetAllActivities: Getting events from google maps.");
        }

        public async static Task GetAllActivities(List<Activity> activities, string latlong, string type, string pageToken = "")
        {
            log.Debug($"GetAllActivities: Getting events from google maps for type {type} and latlong {latlong}");

            try
            {
                // Get data from api
                var http = new HttpClient();
                var url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latlong}&radius=50000&key=" +
                    $"{ ConfigurationManager.AppSettings["GoogleMapsAPIKey"] }&type={type}";
                if(pageToken != "")
                {
                    url += "&pagetoken=" + pageToken;
                }
                var response = await http.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(GoogleMapsModel));
                // Serialize result into a TicketMasterModel object 
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                var googleMapsModel = (GoogleMapsModel)serializer.ReadObject(ms);
                // Error check
                if(googleMapsModel.results == null)
                {
                    throw new Exception($"Could not read data from Google Maps API.{Environment.NewLine}Results from API: {result}");
                }
                // Iterate each google maps result and retrieve the details from the API
                foreach(var res in googleMapsModel.results)
                {
                    var detailsUrl = "https://maps.googleapis.com/maps/api/place/details/json?" +
                        "placeid=" + res.place_id +
                        "&key=" + ConfigurationManager.AppSettings["GoogleMapsAPIKey"];
                    response = await http.GetAsync(detailsUrl);
                    result = await response.Content.ReadAsStringAsync();
                    serializer = new DataContractJsonSerializer(typeof(GoogleMapsDetailModel));
                    ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                    var googleMapsDetailModel = (GoogleMapsDetailModel)serializer.ReadObject(ms);
                    // Error check
                    if(googleMapsDetailModel.result == null)
                    {
                        throw new Exception($"Could not read data from Google Maps API when retrieving details.{Environment.NewLine}" +
                            $"Results from API: {result}");
                    }
                    var openHours = "";
                    if(res.opening_hours != null && res.opening_hours.weekday_text != null)
                    {
                        foreach (var oh in res.opening_hours.weekday_text)
                        {
                            openHours += oh.ToString() + " ";
                        }
                    }

                    var country = (from a in googleMapsDetailModel.result.address_components.AsEnumerable()
                                   where a.types.Contains("country") && a.short_name != null
                                   select a.short_name).FirstOrDefault();
                    if(country != null && country != "DK")
                    {
                        // Other contries not supported
                        continue;
                    }
                    // Create new activity
                    var newActivity = new Activity
                    {
                        Id = Guid.NewGuid(),
                        Title = googleMapsDetailModel.result.name,
                        OpenHours = openHours == "" ? null : openHours,
                        Url = googleMapsDetailModel.result.url,
                        Address = (from a in googleMapsDetailModel.result.address_components.AsEnumerable()
                                   where a.types.Contains("route") && a.long_name != null
                                   select a.long_name).FirstOrDefault() + " " +
                                   (from a in googleMapsDetailModel.result.address_components.AsEnumerable()
                                    where a.types.Contains("street_number") && a.long_name != null
                                    select a.long_name).FirstOrDefault() + " " +
                                   (from a in googleMapsDetailModel.result.address_components.AsEnumerable()
                                    where a.types.Contains("floor") && a.long_name != null
                                    select a.long_name).FirstOrDefault(),
                        PostalCode = (from a in googleMapsDetailModel.result.address_components.AsEnumerable()
                                      where a.types.Contains("postal_code") && a.long_name != null
                                      select a.long_name).FirstOrDefault(),
                        City = (from a in googleMapsDetailModel.result.address_components.AsEnumerable()
                                where a.types.Contains("locality") && a.long_name != null
                                select a.long_name).FirstOrDefault(),
                        Latitude = googleMapsDetailModel.result.geometry != null ?
                                    googleMapsDetailModel.result.geometry.location.lat.ToString() : "",
                        Longitude = googleMapsDetailModel.result.geometry != null ?
                                    googleMapsDetailModel.result.geometry.location.lng.ToString() : "",
                        Website = googleMapsDetailModel.result.website,
                        Description = googleMapsDetailModel.result.place_id
                    };
                    activities.Add(newActivity);
                }
                // Next page if any
                if(googleMapsModel.next_page_token != null && googleMapsModel.next_page_token != "")
                {
                    GetAllActivities(activities, latlong, type, googleMapsModel.next_page_token).Wait();
                }
            }
            catch (Exception e)
            {
                log.Error($"GetAllActivities:" + Environment.NewLine +
                    $"Exception: {e.Message} " + Environment.NewLine +
                    $"StacTrace: {e.StackTrace}");
            }
        }
    }
}
