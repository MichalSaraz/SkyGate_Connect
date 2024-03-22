using Core;
using Core.BaggageContext.Enums;
using Core.FlightContext;
using Core.PassengerContext.Booking;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.PassengerContext.JoinClasses
{
    public class SpecialServiceRequest
    {
        public SSRCode SSRCode { get; private set; }
        public string SSRCodeId { get; private set; }

        public Passenger Passenger { get; private set; }
        public Guid PassengerId { get; private set; }

        public Flight Flight { get; private set; }
        public int FlightId { get; private set; }

        [MaxLength(150)]
        public string? FreeText { get; private set; }

        public SpecialServiceRequest(string sSRCodeId, int flightId, Guid passengerId, string? freeText)
        {
            SSRCodeId = sSRCodeId;
            FlightId = flightId;
            PassengerId = passengerId;
            FreeText = freeText;
        }
    }
}
