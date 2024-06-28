using Newtonsoft.Json;

namespace Core.Dtos
{
    public class InfantOverviewDto : BasePassengerOrItemDto
    {
        [JsonIgnore]
        [JsonProperty(Order = -3)]
        public override string SeatNumberOnCurrentFlight { get; set; }
        
        [JsonProperty(Order = -2)]
        public Guid AssociatedAdultPassengerId { get; set; }

        [JsonProperty(Order = -2)]
        public PassengerFlightDto CurrentFlight { get; set; }
    }
}
