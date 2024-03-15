using Newtonsoft.Json;

namespace Core.Dtos
{
    public class PassengerOverviewDto
    {
        [JsonProperty(Order = -2)]
        public Guid Id { get; set; }

        [JsonProperty(Order = -2)]
        public string FirstName { get; set; }

        [JsonProperty(Order = -2)]
        public string LastName { get; set; }

        [JsonProperty(Order = -2)]
        public string Gender { get; set; }

        [JsonProperty(Order = -2)]
        public string PNR { get; set; }

        [JsonProperty(Order = -2)]
        public int NumberOfCheckedBags { get; set; }

        [JsonProperty(Order = -2)]
        public PassengerFlightDto CurrentFlight { get; set; }

        [JsonProperty(Order = -2)]
        public SeatDto SeatOnCurrentFlight { get; set; }
    }
}
