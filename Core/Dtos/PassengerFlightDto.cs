using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext.Enums;
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
    public class PassengerFlightDto
    {
        public Guid PassengerId { get; set; }
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string DestinationFrom { get; set; }
        public string DestinationTo { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public int? BoardingSequenceNumber { get; set; }        
        public string BoardingZone { get; set; }        
        public string AcceptanceStatus { get; set; }
    }
}
