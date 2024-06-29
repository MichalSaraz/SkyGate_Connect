using Core.PassengerContext.Booking.Enums;

namespace Core.PassengerContext
{
    public class Infant : BasePassengerOrItem
    {
        // The associated passenger must have an age of 18 or higher for the infant to be associated.
        public Passenger AssociatedAdultPassenger { get; set; }
        public Guid AssociatedAdultPassengerId { get; set; }

        public Infant(Guid associatedAdultPassengerId, string firstName, string lastName, PaxGenderEnum gender,
            Guid? bookingDetailsId, int? weight) : base(firstName, lastName, gender, bookingDetailsId, weight)
        {
            AssociatedAdultPassengerId = associatedAdultPassengerId;
        }
    }
}