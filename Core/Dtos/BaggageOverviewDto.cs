using Newtonsoft.Json;

namespace Core.Dtos
{
    public class BaggageOverviewDto : BaggageBaseDto
    {
        [JsonProperty(Order = -2)]
        public Guid Id { get; set; }

        [JsonProperty(Order = -2)]
        public string TagNumber { get; set; }

        [JsonProperty(Order = -2)]
        public int Weight { get; set; }

        [JsonProperty(Order = -2)]
        public string FinalDestination { get; set; }

        [JsonProperty(Order = -2)]
        public string PassengerFirstName { get; set; }

        [JsonProperty(Order = -2)]
        public string PassengerLastName { get; set; }

        [JsonProperty(Order = -2)]
        public string SpecialBagType { get; set; }

        [JsonProperty(Order = -2)]
        public string SpecialBagDescription { get; set; }
        
        [JsonProperty(Order = -2)]
        public string BaggageType { get; init; }
    }
}