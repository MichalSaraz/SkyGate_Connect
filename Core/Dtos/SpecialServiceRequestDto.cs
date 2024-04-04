namespace Core.Dtos
{
    public class SpecialServiceRequestDto
    {
        public Guid Id { get; init; }
        public string SSRCode { get; init; }
        public Guid PassengerOrItemId { get; init; }
        public Guid FlightId { get; init; }
        public string FreeText { get; init; }
    }
}