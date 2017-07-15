using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityFinder
{
    public class Eventdate
    {
        public string format { get; set; }
        public string value { get; set; }
    }

    public class Image
    {
        public string url { get; set; }
        public string type { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Onsale
    {
        public string format { get; set; }
        public string value { get; set; }
    }

    public class Offsale
    {
        public string format { get; set; }
        public string value { get; set; }
    }

    public class Properties
    {
        public bool cancelled { get; set; }
        public bool rescheduled { get; set; }
        public bool seats_avail { get; set; }
        public bool sold_out { get; set; }
        public bool package { get; set; }
        public bool canceled { get; set; }
    }

    public class Address
    {
        public string address { get; set; }
        public string postal_code { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public double @long { get; set; }
        public double lat { get; set; }
    }

    public class Location
    {
        public Address address { get; set; }
    }

    public class Venue
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Location location { get; set; }
    }

    public class Subcategory
    {
        public string name { get; set; }
        public int id { get; set; }
    }

    public class Category
    {
        public string name { get; set; }
        public int id { get; set; }
        public List<Subcategory> subcategories { get; set; }
    }

    public class Attraction
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class ExcludingTicketFees
    {
        public float min { get; set; }
        public float max { get; set; }
    }

    public class IncludingTicketFees
    {
        public float min { get; set; }
        public float max { get; set; }
    }

    public class PriceRanges
    {
        public ExcludingTicketFees excluding_ticket_fees { get; set; }
        public IncludingTicketFees including_ticket_fees { get; set; }
    }

    public class Event
    {
        public string id { get; set; }
        public string domain_id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool externalUrl { get; set; }
        public Eventdate eventdate { get; set; }
        public string day_of_week { get; set; }
        public string timezone { get; set; }
        public string localeventdate { get; set; }
        public List<Image> images { get; set; }
        public Onsale onsale { get; set; }
        public Offsale offsale { get; set; }
        public Properties properties { get; set; }
        public Venue venue { get; set; }
        public List<Category> categories { get; set; }
        public List<Attraction> attractions { get; set; }
        public PriceRanges price_ranges { get; set; }
        public string currency { get; set; }
    }

    public class Pagination
    {
        public int start { get; set; }
        public int rows { get; set; }
        public int total { get; set; }
    }

    public class TicketMasterModel
    {
        public List<Event> events { get; set; }
        public Pagination pagination { get; set; }
    }
}
