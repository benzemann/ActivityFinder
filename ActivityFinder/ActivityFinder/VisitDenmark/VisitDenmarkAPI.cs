using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using log4net;
using ActivityModel;
using System.Text.RegularExpressions;

namespace ActivityFinder.VisitDenmark
{
    public static class VisitDenmarkAPI
    {

        private static readonly ILog log = log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void GetAllActivities(List<Activity> activities, int startIndex=1)
        {
            log.Debug($"GetAllActivities: Getting activities from visitdenmark. startIndex = {startIndex}");

            var web = new HtmlWeb();
            var url = "http://www.visitdenmark.dk/da/search/whatson?ankr-q=%2A&ankr-channels=339&ankr-startIndex=" + startIndex;
            var document = web.Load(url);
            // Get all links
            var links = (from element in document.DocumentNode.Descendants().AsEnumerable()
                         where element.Attributes["class"] != null &&
                         element.Attributes["class"].Value == "ProductImageWrapper" &&
                         element.ChildNodes[1] != null && element.ChildNodes[1].Attributes["href"] != null
                         select element.ChildNodes[1].Attributes["href"].Value).ToList();

            foreach (var link in links)
            {
                document = web.Load(link);
                var newActivity = new Activity { Id = Guid.NewGuid() };
                // Get image
                var image = (from element in document.DocumentNode.Descendants().AsEnumerable()
                             where element.Attributes["class"] != null &&
                             element.Attributes["class"].Value == "mfp-image popup-open-btn" &&
                             element.Attributes["href"] != null
                             select element.Attributes["href"].Value).FirstOrDefault();
                if (image != null)
                {
                    newActivity.Image = image;
                }
                // Get title
                var title = document.DocumentNode.Descendants("h1").Select(x => x.InnerText).FirstOrDefault();
                if (title != null)
                {
                    newActivity.Title = title;
                }
                // Get address
                var address = (from element in document.DocumentNode.Descendants().AsEnumerable()
                               where element.Attributes["class"] != null &&
                               element.Attributes["class"].Value == "gdkProductContacts-item-right"
                               select (from child in element.ChildNodes.AsEnumerable()
                                       where child.Name == "div"
                                       select child.InnerText)).FirstOrDefault().ToList();
                if (address != null && address.Count > 1)
                {
                    newActivity.Address = address[0];
                    newActivity.PostalCode = address[1].Split(' ')[0];
                    newActivity.City = address[1].Split(' ')[1];
                    // Get Latitude / Longitude
                    try
                    {
                        var latlong = Helper.LatLongFromAddress(newActivity.Address + ", " + newActivity.PostalCode + " " + newActivity.City);
                        if (latlong != null)
                        {
                            latlong.Wait();
                            newActivity.Latitude = latlong.Result.Lat;
                            newActivity.Longitude = latlong.Result.Long;
                        }
                    } catch(Exception e)
                    {
                        log.Error("Could not get latitude longitude information from visitdenmark." + Environment.NewLine +
                            "Address: " + newActivity.Address + ", " + newActivity.PostalCode + " " + newActivity.City + Environment.NewLine +
                            "Message: " + e.Message + Environment.NewLine + "StackTrace: " + e.StackTrace + Environment.NewLine);
                    }
                    
                }
                // Get website
                var website = (from element in document.DocumentNode.Descendants().AsEnumerable()
                               where element.Attributes["class"] != null &&
                               element.Attributes["class"].Value == "gdkProductContacts-item-right"
                               select (from child in element.ChildNodes.AsEnumerable()
                                       where child.Name == "a" &&
                                       child.Attributes["href"] != null
                                       select child.Attributes["href"].Value).FirstOrDefault()).ToList();
                if(website != null && website.Count > 1 && website[1] != null && website[1].StartsWith("mailto") == false)
                {
                    newActivity.Website = website[1];
                }
                // Get category
                var category = 
                    document.DocumentNode
                    .SelectNodes("*//div/section/section[2]/div/div/div/div//div/ul/li/ul/li");

                if (category != null && category.FirstOrDefault() != null)
                {
                    newActivity.Category = category.FirstOrDefault().InnerText;
                }
                // Openhours
                var regex = new Regex(@"<td>(.+)<\/td>");
                var openHours = document.DocumentNode
                    .SelectNodes("//*//div/section/section[1]/div[2]/div[5]/div").FirstOrDefault().InnerText;
                newActivity.OpenHours = openHours
                    .Replace("\n", "").Replace("\t", "").Replace("Åbningstider", "")
                    .Trim().Replace("                          ", " ").Replace("                    ", " ");// TODO: LINE BREAK NOT WORKING.Replace("    ","<br/>");

                // Url
                newActivity.Url = link;
                // Add activity to list
                activities.Add(newActivity);
            }
            // If links where found, try to go to next page
            if(links.Count > 0)
            {
                GetAllActivities(activities, startIndex + 21);
            }
            log.Debug("GetAllActivities: Ended");
        }

    }
}
