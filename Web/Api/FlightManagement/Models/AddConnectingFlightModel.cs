using System.ComponentModel.DataAnnotations;
using Core.SeatingContext.Enums;

namespace Web.Api.FlightManagement.Models
{
    public class AddConnectingFlightModel
    {
        [Required]
        public required string AirlineId { get; set; }
        
        [Required]
        public required string FlightNumber { get; set; }

        [Required]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])(?i)(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)$", 
            ErrorMessage = "Date must be in the format dMMM or DDMMM")]
        public required string DepartureDate { get; set; }

        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$",
            ErrorMessage = "Time must be in the format HH:MM or H:MM")]
        public required string DepartureTime { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Destination must be in the format XXX")]
        public required string DestinationFrom { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Destination must be in the format XXX")]
        public required string DestinationTo { get; set; }
        
        [Required]
        public FlightClassEnum FlightClass { get; set; }
    }
}
