using Newtonsoft.Json;

namespace Core.Dtos
{
    public class FlightOverviewDto
    {
        [JsonProperty(Order = -2)]
        public Guid Id { get; init; }

        [JsonProperty(Order = -2)]
        public string FlightNumber { get; init; }

        [JsonProperty(Order = -2)]
        public DateTime DepartureDateTime { get; init; }

        [JsonProperty(Order = -2)]
        public DateTime ArrivalDateTime { get; init; }

        [JsonProperty(Order = -2)]
        public string DestinationFrom { get; init; }

        [JsonProperty(Order = -2)]
        public string DestinationTo { get; init; }
    }
}
