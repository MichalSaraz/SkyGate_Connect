using Core.PassengerContext.Booking.Enums;
using Core.SeatingContext.Enums;

namespace Web.Api.PassengerManagement.Models
{
    public class NoRecPassengerModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public PaxGenderEnum Gender { get; set; }
        public int BaggageAllowance { get; set; }
        public bool PriorityBoarding { get; set; }
        public string? PNRId { get; set; }
        public bool IsChild { get; set; }

        //Key as a flight number and value as a departure date in the format "ddMMM"
        public Dictionary<string, string> Flights { get; set; } = new();
        public Dictionary<string, FlightClassEnum> BookedClass { get; set; } = new();
    }
}