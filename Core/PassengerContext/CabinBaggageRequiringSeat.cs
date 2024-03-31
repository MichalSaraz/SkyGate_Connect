using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext;

namespace Core.PassengerContext
{
    public class CabinBaggageRequiringSeat : BasePassengerOrItem
    {
        public CabinBaggageRequiringSeat(string firstName, string lastName, PaxGenderEnum gender, Guid bookingDetailsId, int? weight)
            : base(firstName, lastName, gender, bookingDetailsId, weight)
        {
        }
    }
}
