namespace Core.Dtos
{
    public class PassengerFlightDto
    {
        public Guid PassengerId { get; init; }
        public int FlightId { get; init; }
        public string FlightNumber { get; init; }
        public string DestinationFrom { get; init; }
        public string DestinationTo { get; init; }
        public DateTime DepartureDateTime { get; init; }
        public DateTime? ArrivalDateTime { get; init; }
        public int? BoardingSequenceNumber { get; init; }        
        public string BoardingZone { get; init; }        
        public string AcceptanceStatus { get; init; }
    }
}
