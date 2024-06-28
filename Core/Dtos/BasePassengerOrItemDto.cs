using Newtonsoft.Json;

namespace Core.Dtos
{
    public class BasePassengerOrItemDto
    {
        [JsonProperty(Order = -3)]
        public Guid Id { get; set; }

        [JsonProperty(Order = -3)]
        public string FirstName { get; set; }

        [JsonProperty(Order = -3)]
        public string LastName { get; set; }

        [JsonProperty(Order = -3)]
        public string Gender { get; set; }
        
        [JsonProperty(Order = -3)]
        public string PNR { get; set; }
        
        [JsonProperty(Order = -3)]
        public virtual string SeatNumberOnCurrentFlight { get; set; }
        
        [JsonProperty(Order = -3)]
        public string Type { get; set; }
    }
}
