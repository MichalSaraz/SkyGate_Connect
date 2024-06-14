using Core.PassengerContext.Booking.Enums;

namespace Core.PassengerContext
{
    public class CabinBaggageRequiringSeat : BasePassengerOrItem
    {
        public CabinBaggageRequiringSeat(string firstName, string lastName, PaxGenderEnum gender,
            Guid? bookingDetailsId, int? weight) : base(firstName, lastName, gender, bookingDetailsId, weight)
        {
        }
    }
}