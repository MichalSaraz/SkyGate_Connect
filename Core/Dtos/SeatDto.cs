namespace Core.Dtos
{
    public class SeatDto
    {
        public Guid Id { get; init; }
        public string SeatNumber { get; init; }
        public string FlightClass { get; init; }
        public string SeatStatus { get; init; }
        public string SeatType { get; init; }
        public Guid? PassengerId { get; init; }
        public string PassengerFirstName { get; init; }
        public string PassengerLastName { get; init; }
        public int FlightId { get; init; }
    }
}
