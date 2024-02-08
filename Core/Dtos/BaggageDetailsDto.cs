using Core.BaggageContext.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class BaggageDetailsDto : BaggageOverviewDto
    {
        public string PassengerFirstName { get; set; }
        public string PassengerLastName { get; set; }
        public string SpecialBagType { get; set; }
        public string SpecialBagDescription { get; set; }
        public string BaggageType { get; set; }
        public List<string> Flights { get; set; } = new List<string>();
    }
}
