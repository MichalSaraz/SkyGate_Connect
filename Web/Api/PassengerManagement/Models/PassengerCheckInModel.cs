using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext.Enums;

namespace Web.Api.PassengerManagement.Models
{
    public class PassengerCheckInModel
    {
        public List<Guid> PassengerIds { get; set; } = new();
        public List<Guid> FlightIds { get; set; } = new();

        public SeatPreferenceEnum SeatPreference { get; set; } = SeatPreferenceEnum.None;
        
        public NotTravellingReasonEnum? NotTravellingReason { get; set; }
        public int? Weight { get; set; }
    }
}
