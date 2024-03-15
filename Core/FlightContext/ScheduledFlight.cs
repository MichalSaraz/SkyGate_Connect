namespace Core.FlightContext
{
    public class ScheduledFlight
    {
        public string FlightNumber { get; set; }
        
        public string[] Codeshare { get; set; }

        public List<KeyValuePair<DayOfWeek, TimeSpan>> DepartureTimes { get; set; } = new();
        public List<KeyValuePair<DayOfWeek, TimeSpan>> ArrivalTimes { get; set; } = new();
        public List<KeyValuePair<DayOfWeek, TimeSpan>> FlightDuration { get; set; } = new();
        
        public string DestinationFrom { get; set; }
        public string DestinationTo { get; set; }        
        public string Airline { get; set; }                
    }
}
