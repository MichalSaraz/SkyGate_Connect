using Newtonsoft.Json;

namespace Core.Dtos;

public class ItemOverviewDto : BasePassengerOrItemDto
{
    [JsonProperty(Order = -2)]
    public PassengerFlightDto CurrentFlight { get; set; }

    [JsonProperty(Order = -2)]
    public SeatDto SeatOnCurrentFlightDetails { get; set; }
}