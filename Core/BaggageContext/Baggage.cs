using Core.FlightContext;
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
        public int Id { get; private set; }

        [Required]
        public Passenger Passenger { get; private set; }
        public Guid PassengerId { get; private set; }

        [Required]
        public BaggageTag BaggageTag { get; private set; }

        public SpecialBag SpecialBag { get; private set; }


        [Range(1, 32)] //upravit pro americkou soustavu
        public int Weight { get; set; }

        public bool IsSpecialBag { get; private set; } = false;

        public bool IsActive { get; private set; } = false;


        public BaggageTypeEnum BaggageType { get; private set; } = BaggageTypeEnum.Local;

        public List<FlightBaggage> Flights { get; private set; } = new List<FlightBaggage>();       
    }
}
