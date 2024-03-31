namespace Core.Dtos
{
    public class PassengerDetailsDto : PassengerOverviewDto
    {
        public int BaggageAllowance { get; init; }
        public bool PriorityBoarding { get; init; }
        public string FrequentFlyerNumber { get; init; }
        public List<APISDataDto> TravelDocuments { get; init; } = new();
        public List<BaggageOverviewDto> PassengerCheckedBags { get; init; } = new();
        public List<CommentDto> Comments { get; init; } = new();
        public List<SpecialServiceRequestDto> SpecialServiceRequests { get; init; } = new();
        public List<PassengerFlightDto> ConnectingFlights { get; init; }
        public List<PassengerFlightDto> InboundFlights { get; init; }
        public List<SeatDto> OtherFlightsSeats { get; init; }
    }
}
