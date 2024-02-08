using Core.BaggageContext.Enums;
using Core.FlightContext;
using Core.FlightContext.FlightInfo;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BaggageContext
{
    public class Baggage
    {
        public Guid Id { get; private set; }

        [Required]
        public Passenger Passenger { get; set; }
        public Guid PassengerId { get; set; }

        public BaggageTag? BaggageTag { get; set; }

        public SpecialBag? SpecialBag { get; set; }

        public Destination FinalDestination { get; set; }
        public string DestinationId { get; set; }

        [Range(1, 32)]
        public int Weight { get; set; }

        //public bool IsSpecialBag { get; private set; } = false;

        //public bool IsActive { get; private set; } = false;               

        public List<FlightBaggage> Flights { get; set; } = new List<FlightBaggage>();       
    }
}
