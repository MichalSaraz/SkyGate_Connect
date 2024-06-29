using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.PassengerContext.Booking
{
    public class PassengerBookingDetails
    {
        [Required]
        public Guid Id { get; private set; }

        [Required]
        public string FirstName { get; private set; }

        [Required]
        public string LastName { get; private set; }

        [Required]
        public PaxGenderEnum Gender { get; private set; }

        [Required]
        public BookingReference PNR { get; }
        public string PNRId { get; private set; }

        public int? Age { get; }

        public int BaggageAllowance { get; }
        public bool PriorityBoarding { get; }
        public string FrequentFlyerCardNumber { get; }

        public BasePassengerOrItem PassengerOrItem { get; }
        public Guid? PassengerOrItemId { get; set; }

        public PassengerBookingDetails AssociatedPassengerBookingDetails { get; }
        public Guid? AssociatedPassengerBookingDetailsId { get; set; }

        public Dictionary<string, FlightClassEnum> BookedClass { get; private set; } = new();
        public Dictionary<string, string> ReservedSeats { get; set; } = new();
        public Dictionary<string, List<string>> BookedSSR { get; set; } = new();

        public PassengerBookingDetails()
        {
        }

        public PassengerBookingDetails(string firstName, string lastName, PaxGenderEnum gender, string pNRId)
        {
            Id = new Guid();
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            PNRId = pNRId;
        }
    }
}
