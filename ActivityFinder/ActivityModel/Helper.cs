using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Configuration;

namespace ActivityModel
{
    public static class Helper
    {
        public class LatLong
        {
            public float Lat;
            public float Long;
        }
        public static async Task<LatLong> LatLongFromAddress(string address)
        {
            var http = new HttpClient();
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={address}" +
                ", Danmark&key=" + ConfigurationManager.AppSettings["GoogleMapsAPIKey"];
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(GoogleAddressModel));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var googleAddress = (GoogleAddressModel)serializer.ReadObject(ms);

            if(googleAddress.results == null)
            {
                return null;
            }

            return new LatLong
            {
                Lat = (float)googleAddress.results.FirstOrDefault().geometry.location.lat,
                Long = (float)googleAddress.results.FirstOrDefault().geometry.location.lng
            };
        }

    }
}
