namespace Core.Dtos
{
    public class BaggageDetailsDto : BaggageOverviewDto
    {
        public List<FlightBaggageDto> Flights { get; init; } = new();
    }
}
