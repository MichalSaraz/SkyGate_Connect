namespace Core.Dtos
{
    public class SpecialServiceRequestDto
    {
        public string SSRCode { get; init; }
        public Guid PassengerOrItemId { get; init; }
        public Guid FlightId { get; init; }
        public string FreeText { get; init; }
    }
}