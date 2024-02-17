using Core.PassengerContext.Booking.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
