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
    public class SeatDto
    {
        public Guid Id { get; set; }
        public string SeatNumber { get; set; }
        public string FlightClass { get; set; }
        public string SeatStatus { get; set; }
        public string SeatType { get; set; }
        public Guid? PassengerId { get; set; }
        public string PassengerFirstName { get; set; }
        public string PassengerLastName { get; set; }
        public int FlightId { get; set; }
    }
}
