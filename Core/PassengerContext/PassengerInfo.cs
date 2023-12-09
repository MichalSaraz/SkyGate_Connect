using Core.PassengerContext.Booking;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext;
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
        public int Id { get; protected set; }

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

        public int BaggageAllowance { get; protected set; } = 0;

        public bool PriorityBoarding { get; protected set; } = false;

        public FrequentFlyer? FrequentFlyer { get; protected set; }
        public int? FrequentFlyerId { get; protected set; }

        public Dictionary<string, string> ReservedSeats { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, List<string>> BookedSSR { get; set; } = new Dictionary<string, List<string>>();

        public List<string> BookedSSRList { get; set; } = new List<string>();

        public PassengerInfo()
        {
        }

        public PassengerInfo(string firstName, string lastName, PaxGenderEnum gender, string? pNRId, int? id = null)
        {
            Id = id ?? 0;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            PNRId = pNRId;
        }
    }
}
