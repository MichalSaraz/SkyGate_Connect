namespace Core.Dtos
{
    public class PassengerFlightDto
    {
        public Guid PassengerOrItemId { get; init; }
        public Guid FlightId { get; init; }
        public string FlightNumber { get; init; }
        public string DestinationFrom { get; init; }
        public string DestinationTo { get; init; }
        public DateTime DepartureDateTime { get; init; }
        public DateTime? ArrivalDateTime { get; init; }
        public string FlightClass { get; init; }
        public int? BoardingSequenceNumber { get; init; }        
        public string BoardingZone { get; init; }        
        public string AcceptanceStatus { get; init; }
        public string NotTravellingReason { get; init; }
    }
}
