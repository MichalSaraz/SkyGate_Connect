using Core.FlightContext;
using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext.Enums;

namespace Core.PassengerContext.JoinClasses
{
    public class PassengerFlight
    {
        public BasePassengerOrItem PassengerOrItem { get; }
        public Guid PassengerOrItemId { get; private set; }

        public BaseFlight Flight { get; }
        public Guid FlightId { get; private set; }

        // is null when AcceptanceStatus is NotAccepted
        public int? BoardingSequenceNumber { get; set; }

        // is null when AcceptanceStatus is NotAccepted
        public BoardingZoneEnum? BoardingZone { get; set; }
        public FlightClassEnum FlightClass { get; set; }
        public AcceptanceStatusEnum AcceptanceStatus { get; set; } = AcceptanceStatusEnum.NotAccepted;
        
        public NotTravellingReasonEnum? NotTravellingReason { get; set; }
        
        public PassengerFlight(
            Guid passengerOrItemId,
            Guid flightId,
            FlightClassEnum flightClass)

        {
            PassengerOrItemId = passengerOrItemId;
            FlightId = flightId;
            FlightClass = flightClass;
        }
    }
}
