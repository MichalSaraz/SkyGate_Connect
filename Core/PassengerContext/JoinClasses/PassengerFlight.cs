using Core.FlightContext;
using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext.Enums;
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
        public Guid PassengerId { get; set; }

        public Flight Flight { get; set; }
        public int FlightId { get; set; }

        public int? BoardingSequenceNumber { get; set; }

        public BoardingZoneEnum? BoardingZone { get; set; }

        public AcceptanceStatusEnum AcceptanceStatus { get; set; } = AcceptanceStatusEnum.NotAccepted;
    }
}
