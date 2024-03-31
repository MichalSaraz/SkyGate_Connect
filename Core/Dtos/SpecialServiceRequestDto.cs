namespace Core.Dtos
{
    public class SpecialServiceRequestDto
    {
        public Guid Id { get; init; }
        public string SSRCode { get; init; }
        public Guid PassengerId { get; init; }
        public int FlightId { get; init; }
        public string FreeText { get; init; }
    }
}