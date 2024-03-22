using System.ComponentModel.DataAnnotations;
using Core.SeatingContext.Enums;

namespace Web.Api.PassengerContext.Models
{
    public class AddConnectingFlightModel
    {
        public string AirlineId { get; set; }
        public string FlightNumber { get; set; }

        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])(?i)(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)$", 
            ErrorMessage = "Date must be in the format dMMM or DDMMM")]
        public string DepartureDate { get; set; }

        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Destination must be in the format XXX")]
        public string DestinationFrom { get; set; }

        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Destination must be in the format XXX")]
        public string DestinationTo { get; set; }
        public FlightClassEnum FlightClass { get; set; }
    }
}
