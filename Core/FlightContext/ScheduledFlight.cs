using Core.FlightContext.FlightInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FlightContext
{
    public class ScheduledFlight
    {
        public string FlightNumber { get; private set; }
        
        public string[] Codeshare { get; private set; }

        public List<KeyValuePair<DayOfWeek, TimeSpan>> DepartureTimes { get; private set; } = new List<KeyValuePair<DayOfWeek, TimeSpan>>();
        public List<KeyValuePair<DayOfWeek, TimeSpan>> ArrivalTimes { get; private set; } = new List<KeyValuePair<DayOfWeek, TimeSpan>>();
        public List<KeyValuePair<DayOfWeek, TimeSpan>> FlightDuration { get; private set; } = new List<KeyValuePair<DayOfWeek, TimeSpan>>();
        
        public string DestinationFrom { get; private set; }
        public string DestinationTo { get; private set; }        
        public string Airline { get; private set; }                
    }
}
