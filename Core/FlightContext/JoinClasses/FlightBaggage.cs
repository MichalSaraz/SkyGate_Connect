using Core.BaggageContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FlightContext.JoinClasses
{
    public class FlightBaggage
    {
        public Flight Flight { get; private set; }
        public int FlightId { get; private set; }

        public Baggage Baggage { get; private set; }
        public int BaggageId { get; private set; }
    }
}
