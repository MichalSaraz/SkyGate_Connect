using Core.FlightContext;
using Core.PassengerContext.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext.JoinClasses
{
    public class PassengerComment
    {
        public Passenger Passenger { get; private set; }
        public int PassengerId { get; private set; }

        public Comment Comment { get; private set; }
        public int CommentId { get; private set; }
    }
}
