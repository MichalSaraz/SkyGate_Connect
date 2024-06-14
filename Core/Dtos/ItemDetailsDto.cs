namespace Core.Dtos;

public class ItemDetailsDto : ItemOverviewDto
{
    public int? Weight { get; init; }
    public List<PassengerFlightDto> ConnectingFlights { get; init; }
    public List<PassengerFlightDto> InboundFlights { get; init; }
    public List<SeatDto> OtherFlightsSeats { get; init; }
}