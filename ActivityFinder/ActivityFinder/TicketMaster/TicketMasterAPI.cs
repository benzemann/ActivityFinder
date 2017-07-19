using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Configuration;
using log4net;
using ActivityModel;

namespace ActivityFinder
{
    static class TicketMasterAPI
    {

        private static readonly ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public async static Task GetAllActivities(List<Activity> activities, int pageNumber=0)
        {
            log.Debug($"GetAllActivities: Getting events from ticketmaster. Page = {pageNumber}");
            try
            {
                // Get data from api
                var http = new HttpClient();
                var url = $"https://app.ticketmaster.eu/mfxapi/v1/events?domain_ids=denmark&rows=250&apikey=" +
                    $"{ ConfigurationManager.AppSettings["TicketMasterAPIKey"] }&start={pageNumber}";
                var response = await http.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(TicketMasterModel));
                // Serialize result into a TicketMasterModel object 
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                var ticketMasterModel = (TicketMasterModel)serializer.ReadObject(ms);
                // Check for errors 
                if (ticketMasterModel.events == null)
                {
                    throw new Exception($"Could not read data from Ticketmaster API.{Environment.NewLine}Result from API: {result}");
                }
                // Create and add all events from ticketmaster
                foreach(var tmEvent in ticketMasterModel.events)
                {
                    var newActivity = new Activity
                    {
                        Id = Guid.NewGuid(),
                        Title = tmEvent.name,
                        Image = tmEvent.images != null && tmEvent.images.Count > 0 ? 
                        tmEvent.images.FirstOrDefault().url : "",
                        StartDate = tmEvent.eventdate != null ? DateTime.Parse(tmEvent.eventdate.value).ToLocalTime().ToString() : "",
                        Price = tmEvent.price_ranges != null ? (tmEvent.price_ranges.including_ticket_fees != null ?
                        tmEvent.price_ranges.including_ticket_fees.min + " kr." +
                        " til " + tmEvent.price_ranges.including_ticket_fees.max + " kr." : "") : "",
                        Url = tmEvent.url,
                        Address = tmEvent.venue != null ? 
                        (tmEvent.venue.location.address != null ? tmEvent.venue.location.address.address : "") : "",
                        PostalCode = tmEvent.venue != null ? 
                        (tmEvent.venue.location.address != null ? tmEvent.venue.location.address.postal_code : "") : "",
                        City = tmEvent.venue != null ? 
                        (tmEvent.venue.location.address != null ? tmEvent.venue.location.address.city : "") : "",
                        Latitude = tmEvent.venue != null ? 
                        (tmEvent.venue.location.address != null ? tmEvent.venue.location.address.lat : 0.0) : 0.0,
                        Longitude = tmEvent.venue != null ? 
                        (tmEvent.venue.location.address != null ? tmEvent.venue.location.address.@long : 0.0) : 0.0,
                        Category = tmEvent.categories != null ? tmEvent.categories.FirstOrDefault().name : "",
                        Website = tmEvent.url
                    };
                    activities.Add(newActivity);
                }
                // Get events from the next page if not on the last page
                if(pageNumber + 250 < ticketMasterModel.pagination.total)
                {
                    GetAllActivities(activities, pageNumber + 250).Wait();
                }
            } catch(Exception e)
            {
                log.Error($"GetAllActivities:" + Environment.NewLine +
                    $"Exception: {e.Message} " + Environment.NewLine +
                    $"StacTrace: {e.StackTrace}");
            }
        }
    }
}
