using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;

namespace Core.PassengerContext
{
    public class Infant : BasePassengerOrItem
    {
        // The associated passenger must have an age of 18 or higher for the infant to be associated.
        public Passenger AssociatedAdultPassenger { get; set; }
        public Guid AssociatedAdultPassengerId { get; set; }        

        public Infant(Guid associatedAdultPassengerId, string firstName, string lastName, PaxGenderEnum gender, Guid bookingDetailsId, int? weight) 
            : base(firstName, lastName, gender, bookingDetailsId, weight)
        {
            AssociatedAdultPassengerId = associatedAdultPassengerId;
        }

        public override void MapFromPassengerBookingDetails(PassengerBookingDetails passengerBookingDetails)
        {
            base.MapFromPassengerBookingDetails(passengerBookingDetails);

            AssociatedAdultPassengerId = passengerBookingDetails.AssociatedPassengerBookingDetailsId.HasValue
                ? passengerBookingDetails.AssociatedPassengerBookingDetailsId.Value
                : Guid.Empty;
        }
    }
}
