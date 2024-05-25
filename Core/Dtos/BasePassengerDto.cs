using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class BasePassengerDto
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
        public string SeatNumberOnCurrentFlight { get; set; }

        [JsonProperty(Order = -3)]
        public InfantDto Infant { get; set; }
    }
}
