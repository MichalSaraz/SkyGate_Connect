namespace Core.Dtos
{
    public class SpecialServiceRequestDto
    {
        public string SSRCode { get; init; }
        public Guid PassengerId { get; init; }
        public Guid FlightId { get; init; }
        public string FlightNumber { get; set; }
        public string FreeText { get; init; }
    }
}