using Core.PassengerContext.Booking.Enums;
using Core.PassengerContext.Booking;
using Core.PassengerContext.JoinClasses;
using Core.PassengerContext.APIS;
using Core.SeatingContext;

namespace Core.PassengerContext
{
    public abstract class BasePassengerOrItem
    {
        public Guid Id { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public PaxGenderEnum Gender { get; protected set; }

        // might be null only when AcceptanceStatus is NotAccepted
        public int? Weight { get; set; }

        public PassengerBookingDetails BookingDetails { get; protected set; }
        public Guid? BookingDetailsId { get; protected set; }

        public List<APISData> TravelDocuments { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Seat> AssignedSeats { get; set; }
        public List<PassengerFlight> Flights { get; set; } = new();

        protected BasePassengerOrItem(string firstName, string lastName, PaxGenderEnum gender, Guid? bookingDetailsId,
            int? weight)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            BookingDetailsId = bookingDetailsId;
            Weight = weight;
        }

        protected virtual void MapFromPassengerBookingDetails(PassengerBookingDetails passengerBookingDetails)
        {
            Id = new Guid();
            FirstName = passengerBookingDetails.FirstName;
            LastName = passengerBookingDetails.LastName;
            Gender = passengerBookingDetails.Gender;
        }
    }
}