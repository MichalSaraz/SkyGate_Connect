using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext;
using Core.SeatingContext.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext
{
    public class PassengerInfo
    {
        public Guid Id { get; protected set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public PaxGenderEnum Gender { get; set; }

        [Required]
        public BookingReference PNR { get; set; }
        public string PNRId { get; set; }

        public int Age { get; set; }

        public int BaggageAllowance { get; set; } = 0;

        public bool PriorityBoarding { get; set; } = false;             

        public FrequentFlyer? FrequentFlyer { get; set; }
        public int? FrequentFlyerId { get; set; }

        public Dictionary<string, string> ReservedSeats { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, List<string>> BookedSSR { get; set; } = new Dictionary<string, List<string>>();
        

        public PassengerInfo()
        {
        }

        public PassengerInfo(string firstName, string lastName, PaxGenderEnum gender, string? pNRId)
        {            
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            PNRId = pNRId;
        }
    }
}
