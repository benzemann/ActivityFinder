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
                var url = $"https://app.ticketmaster.eu/discovery/v2/events?apikey=" +
                    $"{ ConfigurationManager.AppSettings["TicketMasterAPIKey"] }&page={pageNumber}&countryCode=DK";
                var response = await http.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(TicketMasterModel));
                // Serialize result into a TicketMasterModel object 
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                var ticketMasterModel = (TicketMasterModel)serializer.ReadObject(ms);
                // Check for errors 
                if (ticketMasterModel._embedded == null || ticketMasterModel._links == null ||
                    ticketMasterModel.page == null)
                {
                    throw new Exception($"Could not read data from Ticketmaster API.{Environment.NewLine}Result from API: {result}");
                }
                // Create and add all events from ticketmaster
                foreach(var tmEvent in ticketMasterModel._embedded.events)
                {
                    var newActivity = new Activity
                    {
                        Id = Guid.NewGuid(),
                        Title = tmEvent.name,
                        Image = tmEvent.images != null && tmEvent.images.Count > 0 ? 
                        tmEvent.images.FirstOrDefault().url : "",
                        StartDate = tmEvent.dates != null ? tmEvent.dates.start.dateTime : "",
                        Price = tmEvent.priceRanges != null ? tmEvent.priceRanges.ToString() : "",
                        Url = tmEvent.url,
                        Address = tmEvent._embedded.venues != null && tmEvent._embedded.venues.Count > 0 ?
                        tmEvent._embedded.venues.FirstOrDefault().address.line1 : "",
                        PostalCode = tmEvent._embedded.venues != null && tmEvent._embedded.venues.Count > 0 ?
                        tmEvent._embedded.venues.FirstOrDefault().postalCode : "",
                        City = tmEvent._embedded.venues != null && tmEvent._embedded.venues.Count > 0 ?
                        tmEvent._embedded.venues.FirstOrDefault().city.name : ""
                    };
                    activities.Add(newActivity);
                }
                // Get events from the next page if not on the last page
                if(pageNumber < ticketMasterModel.page.totalPages - 1)
                {
                    GetAllActivities(activities, ++pageNumber).Wait();
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
