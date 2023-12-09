using Core.FlightContext;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext.JoinClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext.Booking
{
    [NotMapped]
    public class SpecialServiceRequest
    {
        public int Id { get; private set; }

        public SSRCode SSRCode { get; private set; }
        public string SSRCodeId { get; private set; } 
        
        public Flight Flight { get; private set; }
        public int FlightId { get; private set; }


        [MaxLength(150)]
        public string FreeText { get; private set; }
    }
}
