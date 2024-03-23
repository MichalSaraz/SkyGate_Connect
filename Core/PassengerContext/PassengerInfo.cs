using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.PassengerContext
{
    public class PassengerInfo
    {
        [Required]
        public Guid Id { get; protected set; }

        [Required]
        public string FirstName { get; protected set; }

        [Required]
        public string LastName { get; protected set; }

        [Required]
        public PaxGenderEnum Gender { get; protected set; }

        [Required]
        public BookingReference PNR { get; protected set; }
        public string PNRId { get; protected set; }

        public int Age { get; protected set; }

        public int BaggageAllowance { get; protected set; }

        public bool PriorityBoarding { get; protected set; }             

        public FrequentFlyer FrequentFlyer { get; protected set; }
        public int FrequentFlyerId { get; protected set; }

        public Dictionary<string, FlightClassEnum> BookedClass { get; private set; } = new();
        
        public Dictionary<string, string> ReservedSeats { get; set; } = new();

        public Dictionary<string, List<string>> BookedSSR { get; set; } = new();
        
        public PassengerInfo()
        {
        }

        public PassengerInfo(string firstName, string lastName, PaxGenderEnum gender, string pNRId)
        { 
            Id = new Guid();
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            PNRId = pNRId;
        }
    }
}
