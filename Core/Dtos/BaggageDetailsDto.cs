namespace Core.Dtos
{
    public class BaggageDetailsDto : BaggageBaseDto
    {
        public List<FlightBaggageDto> Flights { get; init; } = new();
    }
}
