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
        public Passenger Passenger { get; private set; }
        public int PassengerId { get; private set; }

        public Flight Flight { get; private set; }
        public int FlightId { get; private set; }        
    }
}
