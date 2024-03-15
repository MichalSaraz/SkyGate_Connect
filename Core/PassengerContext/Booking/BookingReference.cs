using System.ComponentModel.DataAnnotations;

namespace Core.PassengerContext.Booking
{
    public class BookingReference
    {
        [Key]
        [RegularExpression("^[A-Z0-9]{6}$")]
        public string PNR { get; private set; }
        
        [Required]
        public List<PassengerInfo> LinkedPassengers { get; set; } = new();
        
        public List<KeyValuePair<string, DateTime>> FlightItinerary { get; set; } = new();
    }
}
