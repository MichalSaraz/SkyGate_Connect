using Core.PassengerContext.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext.JoinClasses
{
    public class PassengerSpecialServiceRequest
    {
        public Passenger Passenger { get; private set; }
        public int PassengerId { get; private set; }

        public SpecialServiceRequest SpecialServiceRequest { get; private set; }
        public int SpecialServiceRequestId { get; private set; }
    }
}
