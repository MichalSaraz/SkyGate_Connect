using System.ComponentModel.DataAnnotations;

namespace Core.PassengerContext.Booking
{
    public class BookingReference
    {
        [Key]
        [RegularExpression("^[A-Z0-9]{6}$")]
        public string PNR { get; set; }
        
        [Required]
        public List<PassengerBookingDetails> LinkedPassengers { get; set; } = new();
        
        public List<KeyValuePair<string, DateTime>> FlightItinerary { get; set; } = new();
    }
}
