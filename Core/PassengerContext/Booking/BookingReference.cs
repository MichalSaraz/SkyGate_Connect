using Core.FlightContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.PassengerContext.Booking
{
    public class BookingReference
    {
        [Key]
        [RegularExpression("^[A-Z0-9]{6}$")]
        public string PNR { get; private set; }
        
        [Required]
        public List<PassengerInfo> LinkedPassengers { get; set; } = new List<PassengerInfo>();
        
        public List<KeyValuePair<string, DateTime>> FlightItinerary { get; set; } = new List<KeyValuePair<string, DateTime>>();
    }
}
