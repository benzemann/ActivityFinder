using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ActivityModel
{
    public class ActivityContext : DbContext
    {
        public ActivityContext() : base("Name=ActivityModel")
        {

        }
        public DbSet<Activity> Activities { get; set; }
    }
}
