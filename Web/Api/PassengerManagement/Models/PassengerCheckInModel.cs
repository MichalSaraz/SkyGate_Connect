using Core.SeatingContext.Enums;

namespace Web.Api.PassengerManagement.Models
{
    public class PassengerCheckInModel
    {
        public List<Guid> PassengerIds { get; set; }
        public List<Guid> FlightIds { get; set; }

        public SeatPreferenceEnum? SeatPreference { get; set; }
    }
}
