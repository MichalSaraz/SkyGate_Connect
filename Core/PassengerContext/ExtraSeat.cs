using Core.PassengerContext.Booking.Enums;

namespace Core.PassengerContext
{
    public class ExtraSeat : BasePassengerOrItem
    {
        public ExtraSeat(string firstName, string lastName, PaxGenderEnum gender, Guid? bookingDetailsId,
            int? weight = null) : base(firstName, lastName, gender, bookingDetailsId, weight)
        {
        }
    }
}