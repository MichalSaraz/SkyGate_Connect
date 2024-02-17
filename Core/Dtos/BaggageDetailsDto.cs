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
    public class BaggageDetailsDto : BaggageBaseDto
    {
        public List<FlightBaggageDto> Flights { get; set; } = new List<FlightBaggageDto>();
    }
}
