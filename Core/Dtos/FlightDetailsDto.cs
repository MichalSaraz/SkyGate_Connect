using Core.SeatingContext.Enums;

namespace Core.Dtos
{
    public class FlightDetailsDto : FlightOverviewDto
    {        
        public TimeSpan FlightDuration { get; init; }        
        public string[] CodeShare { get; init; } 
        public string ArrivalAirportName { get; init; }
        public string AirlineName { get; init; }
        public string AircraftRegistration { get; init; }
        public string AircraftType { get; init; }
        public string FlightStatus { get; init; }
        public string BoardingStatus { get; init; }
        public int TotalBookedInfants { get; init; }
        public int TotalCheckedInInfants { get; init; }
        public Dictionary<FlightClassEnum, int> BookedPassengers { get; init; }
        public Dictionary<FlightClassEnum, int> StandbyPassengers { get; init; }
        public Dictionary<FlightClassEnum, int> CheckedInPassengers { get; init; }        
        public Dictionary<FlightClassEnum, int> AircraftConfiguration { get; init; }
        public Dictionary<FlightClassEnum, int> CabinCapacity { get; init; }
        public Dictionary<FlightClassEnum, int> AvailableSeats { get; init; }
    }
}
