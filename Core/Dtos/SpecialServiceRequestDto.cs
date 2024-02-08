namespace Core.Dtos
{
    public class SpecialServiceRequestDto
    {
        public int Id { get; set; }
        public string SSRCode { get; set; }
        public Guid PassengerId { get; set; }
        public int FlightId { get; set; }
        public string FreeText { get; set; }
    }
}