namespace Core.Dtos
{
    public class FlightOverviewDto
    {
        public Guid Id { get; init; }
        public string ScheduledFlight { get; init; }
        public DateTime DepartureDateTime { get; init; }
        public DateTime ArrivalDateTime { get; init; }
        public string DestinationFrom { get; init; }
        public string DestinationTo { get; init; }
    }
}
