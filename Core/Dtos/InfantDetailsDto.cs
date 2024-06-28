namespace Core.Dtos
{
    public class InfantDetailsDto : InfantOverviewDto
    {
        public List<PassengerFlightDto> ConnectingFlights { get; init; }
        public List<PassengerFlightDto> InboundFlights { get; init; }
    }
}
