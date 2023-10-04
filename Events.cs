using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOGAste
{
    public class Events  
    {

        //Class "Events" is used by IEventsRepository

        //Properties
        //public string EventMsg {  get; set; }
        public int      EventID     {  get; set; }
        //public DateTime TimeCreated { get; set; }
        public string TimeCreated { get; set; }
        public string   MachineName { get; set; }
        public string   UserID      {  get; set; }

        //public string ThreatLvl { get; set; }

    }
}
