using Core.FlightContext;
using Core.PassengerContext.Booking;
using System.ComponentModel.DataAnnotations;

namespace Core.PassengerContext.JoinClasses
{
    public class SpecialServiceRequest
    {
        public SSRCode SSRCode { get; }
        public string SSRCodeId { get; private set; }

        public Passenger Passenger { get; }
        public Guid PassengerId { get; private set; }

        public Flight Flight { get; }
        public Guid FlightId { get; private set; }

        [MaxLength(150)]
        public string FreeText { get; private set; }

        public SpecialServiceRequest(string sSRCodeId, Guid flightId, Guid passengerId, string freeText)
        {
            SSRCodeId = sSRCodeId;
            FlightId = flightId;
            PassengerId = passengerId;
            FreeText = freeText;
        }
    }
}
