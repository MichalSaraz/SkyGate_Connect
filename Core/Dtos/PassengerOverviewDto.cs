using Newtonsoft.Json;

namespace Core.Dtos
{
    public class PassengerOverviewDto : BasePassengerOrItemDto
    {
        [JsonProperty(Order = -2)]
        public int NumberOfCheckedBags { get; set; }

        [JsonProperty(Order = -2)]
        public PassengerFlightDto CurrentFlight { get; set; }

        [JsonProperty(Order = -2)]
        public SeatDto SeatOnCurrentFlightDetails { get; set; }
    }
}
