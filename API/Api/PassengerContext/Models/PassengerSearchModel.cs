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
        [RegularExpression(@"^([0-9]{1,2}[A-Za-z]{3})|([A-Za-z]{3}[0-9]{1,2})$", ErrorMessage = "Date must be in the format dMMM or DDMMM")]
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
