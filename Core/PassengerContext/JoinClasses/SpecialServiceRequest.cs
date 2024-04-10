using Core.FlightContext;
using Core.PassengerContext.Booking;
using System.ComponentModel.DataAnnotations;

namespace Core.PassengerContext.JoinClasses
{
    public class SpecialServiceRequest
    {
        public SSRCode SSRCode { get; private set; }
        public string SSRCodeId { get; private set; }

        public Passenger PassengerOrItem { get; private set; }
        public Guid PassengerOrItemId { get; private set; }

        public Flight Flight { get; private set; }
        public Guid FlightId { get; private set; }

        [MaxLength(150)]
        public string FreeText { get; private set; }

        public SpecialServiceRequest(string sSRCodeId, Guid flightId, Guid passengerOrItemId, string freeText)
        {
            SSRCodeId = sSRCodeId;
            FlightId = flightId;
            PassengerOrItemId = passengerOrItemId;
            FreeText = freeText;
        }
    }
}
