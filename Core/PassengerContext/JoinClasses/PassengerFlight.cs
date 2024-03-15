using Core.FlightContext;
using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext.Enums;

namespace Core.PassengerContext.JoinClasses
{
    public class PassengerFlight
    {
        public Passenger Passenger { get; private set; }
        public Guid PassengerId { get; private set; }

        public BaseFlight Flight { get; private set; }
        public int FlightId { get; private set; }

        public int? BoardingSequenceNumber { get; set; }

        public BoardingZoneEnum? BoardingZone { get; set; }

        public FlightClassEnum FlightClass { get; private set; }

        public AcceptanceStatusEnum AcceptanceStatus { get; set; } = AcceptanceStatusEnum.NotAccepted;
        
        public PassengerFlight(
            Guid passengerId,
            int flightId,
            FlightClassEnum flightClass)

        {
            PassengerId = passengerId;
            FlightId = flightId;
            FlightClass = flightClass;
        }
    }
}
