namespace Core.Dtos
{
    public class FlightDetailsDto : FlightOverviewDto
    {        
        public string[] CodeShare { get; init; }        
        public string Airline { get; init; }
        public int TotalBookedPassengers { get; init; }
    }
}
