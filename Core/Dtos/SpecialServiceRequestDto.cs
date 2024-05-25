namespace Core.Dtos
{
    public class SpecialServiceRequestDto
    {
        public string SSRCode { get; init; }
        public Guid PassengerId { get; init; }
        public Guid FlightId { get; init; }
        public string FreeText { get; init; }
    }
}