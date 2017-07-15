using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityModel
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Website { get; set; }
        public string OpenHours { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Price { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
    }
}
