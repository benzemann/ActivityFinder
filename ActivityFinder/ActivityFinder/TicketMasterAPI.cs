using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
namespace ActivityFinder
{
    static class TicketMasterAPI
    {
        public async static Task GetAllActivities()
        {
            var http = new HttpClient();
            var response = await http.GetAsync("https://app.ticketmaster.eu/discovery/v2/events?apikey=ArYegqH5e3xnrF8Gu1QdBuESAG9Gkikm&countryCode=DK");
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(TicketMasterModel));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (TicketMasterModel)serializer.ReadObject(ms);
        }
    }
}
