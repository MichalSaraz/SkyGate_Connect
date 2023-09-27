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
        public int Id { get; private set; }

        [Required]
        [RegularExpression("^[A-Z0-9]{6}$")]
        public string PNR { get; private set; }

        [Required]
        public List<Passenger> LinkedPassengers { get; private set; }        
    }
}
