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
    public class SpecialServiceRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        public SSRCode SSRCode { get; set; }
        public string SSRCodeId { get; set; }
        
        public Passenger Passenger { get; set; }
        public Guid PassengerId { get; private set; }        
        
        public Flight Flight { get; set; }
        public int FlightId { get; private set; }

        [MaxLength(150)]
        public string? FreeText { get; private set; }

        public SpecialServiceRequest()
        { 
        }

        public SpecialServiceRequest(SSRCode sSRCode, Flight flight, Passenger passenger, string? freeText)
        {
            SSRCode = sSRCode;
            Flight = flight;
            Passenger = passenger;
            FreeText = freeText;
        }
    }
}
