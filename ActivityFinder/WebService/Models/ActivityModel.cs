using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    public class Activity
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Date { get; set; }
        public string Price { get; set; }
        public string Address { get; set; }
    }
}