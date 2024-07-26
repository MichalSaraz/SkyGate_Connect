using System.ComponentModel.DataAnnotations;

namespace Web.Api.PassengerManagement.Models
{
    public class PassengerSearchModel
    {
        [Required]
        public required string FlightNumber { get; set; }

        [Required]
        public required string AirlineId { get; set; }

        [Required]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])(?i)(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)$", 
            ErrorMessage = "Date must be in the format dMMM or DDMMM")]
        public required string DepartureDate { get; set; }

        public string? DocumentNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z]{2,}$")]
        public string? LastName { get; set; }

        public string? PNR { get; set; }  
        
        public string? DestinationFrom { get; set; }

        public string? DestinationTo { get; set; }  
        
        public string? SeatNumber { get; set; }
    }
}
