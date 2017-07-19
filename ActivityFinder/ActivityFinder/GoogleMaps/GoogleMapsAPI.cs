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
        #region Types
        private static string[] types = new string[] { "bowling_alley", "amusement_park", "aquarium",
        "casino", "movie_theater", "museum", "zoo" }; // Possible types: art_gallery spa
        #endregion
        #region Search position
        private static string[] cities = new string[] {
            "København",
            /*"Aarhus",
            "Aalborg",
            "Odense",
            "Esbjerg",
            "Vejle",
            "Frederiksberg",
            "Randers",
            "Viborg",*/
            "Kolding",
            /*"Silkeborg",
            "Herning",
            "Horsens",*/
            "Roskilde",
            /*"Næstved",
            "Slagelse",
            "Sønderborg",
            "Gentofte",
            "Holbæk",
            "Gladsaxe",
            "Hjørring",
            "Helsingør",
            "Guldborgsund",
            "Frederikshavn",
            "Aabenraa",
            "Køge",
            "Skanderborg",
            "Svendborg",
            "Holstebro",
            "Ringkøbing-Skjern",
            "Haderslev",
            "Rudersdal",
            "Lyngby-Taarbæk",
            "Hvidovre",
            "Faaborg-Midtfyn",
            "Fredericia",
            "Varde",
            "Høje-Taastrup",
            "Hillerød",
            "Greve",
            "Ballerup",
            "Kalundborg",
            "Favrskov",
            "Skive",
            "Hedensted",
            "Vordingborg",
            "Thisted",
            "Frederikssund",
            "Lolland",
            "Vejen",
            "Egedal",
            "Tårnby",
            "Mariagerfjord",
            "Syddjurs",
            "Assens",
            "Gribskov",
            "Ikast-Brande",
            "Bornholm",
            "Fredensborg",
            "Furesø",
            "Jammerbugt",
            "Tønder",
            "Middelfart",
            "Norddjurs",
            "Rødovre",
            "Vesthimmerland",
            "Brønderslev",
            "Faxe",
            "Brøndby",
            "Ringsted",
            "Odsherred",
            "Nyborg",
            "Halsnæs",
            "Sorø",
            "Nordfyn",
            "Rebild",
            "Herlev",
            "Albertslund",
            "Lejre",
            "Billund",
            "Hørsholm",
            "Allerød",
            "Kerteminde",
            "Glostrup",
            "Stevns",
            "Odder",
            "Ishøj",
            "Struer",
            "Solrød",
            "Morsø",
            "Lemvig",
            "Vallensbæk",
            "Dragør",
            "Langeland",
            "Ærø",
            "Samsø",
            "Fanø",
            "Læsø"*/
        }; // mayor cities in DK
        #endregion
        public async static Task GetAllActivities(List<Activity> activities)
        {
            log.Debug($"GetAllActivities: Start iterating all cities and types");
            var googleActivities = new List<Activity>();
            foreach (var type in types)
            {
                foreach(var city in cities)
                {
                    var latlong = await Helper.LatLongFromAddress(city);
                    if(latlong == null)
                    {
                        continue;
                    }
                    await GetAllActivities(
                        googleActivities, 
                        latlong.Lat.ToString(System.Globalization.CultureInfo.InvariantCulture) + 
                        ","+latlong.Long.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        type);
                }
            }
            // Remove duplicates by place_id and remove it from the description field
            activities.AddRange(googleActivities.AsEnumerable().GroupBy(x => x.Description).Select(x => x.FirstOrDefault()).ToList());
            foreach(var a in activities)
            {
                a.Description = null;
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
                    if(googleMapsDetailModel.result.opening_hours != null && googleMapsDetailModel.result.opening_hours.weekday_text != null)
                    {
                        foreach (var oh in googleMapsDetailModel.result.opening_hours.weekday_text)
                        {
                            openHours += oh + Environment.NewLine;
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
                                    googleMapsDetailModel.result.geometry.location.lat : 0.0,
                        Longitude = googleMapsDetailModel.result.geometry != null ?
                                    googleMapsDetailModel.result.geometry.location.lng : 0.0,
                        Website = googleMapsDetailModel.result.website,
                        Description = googleMapsDetailModel.result.place_id,
                        Category = googleMapsDetailModel.result.types != null ?
                        googleMapsDetailModel.result.types.FirstOrDefault() : "",
                        Image = res.photos != null ? 
                        $"https://maps.googleapis.com/maps/api/place/photo?maxwidth=250&photoreference={res.photos.FirstOrDefault().photo_reference}&key={ConfigurationManager.AppSettings["GoogleMapsAPIKey"]}" : ""
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
