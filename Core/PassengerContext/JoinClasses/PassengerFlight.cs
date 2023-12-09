using Core.FlightContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext.JoinClasses
{
    public class PassengerFlight
    {
        public Passenger Passenger { get; set; }
        public int PassengerId { get; set; }

        public Flight Flight { get; set; }
        public int FlightId { get; set; }        
    }
}
