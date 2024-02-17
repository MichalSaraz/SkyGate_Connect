using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class BaggageBaseDto
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
    }
}
