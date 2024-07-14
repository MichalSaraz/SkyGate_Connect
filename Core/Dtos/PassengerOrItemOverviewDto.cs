using Newtonsoft.Json;

namespace Core.Dtos
{
    public class PassengerOrItemOverviewDto : BasePassengerOrItemDto
    {
        [JsonProperty(Order = -2)]
        public int NumberOfCheckedBags { get; set; }

        [JsonProperty(Order = -2)]
        public PassengerFlightDto CurrentFlight { get; set; }

        [JsonProperty(Order = -2)]
        public SeatDto SeatOnCurrentFlightDetails { get; set; }
        
        [JsonProperty(Order = -2)]
        public List<PassengerFlightDto> ConnectingFlights { get; init; }
        
        [JsonProperty(Order = -2)]
        public List<PassengerFlightDto> InboundFlights { get; init; }
        
        [JsonProperty(Order = -2)]
        public List<SeatDto> OtherFlightsSeats { get; init; }
    }
}
