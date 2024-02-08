using System.ComponentModel.DataAnnotations;

namespace API.Api.PassengerContext.Models
{
    public class PassengerSearchModel
    {
        [Required]
        public string FlightNumber { get; set; }

        [Required]
        public string AirlineId { get; set; }

        [Required]
        public DateTime? DepartureDate { get; set; }

        public string DocumentNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z]{2,}$")]
        public string LastName { get; set; }

        public string PNR { get; set; }  
        
        public string DestinationFrom { get; set; }

        public string DestinationTo { get; set; }  
        
        public string SeatNumber { get; set; }
    }
}
