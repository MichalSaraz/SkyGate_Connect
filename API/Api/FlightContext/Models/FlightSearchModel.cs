using System.ComponentModel.DataAnnotations;

namespace API.Api.FlightContext.Models
{
    public class FlightSearchModel
    {        
        public string FlightNumber { get; set; }
        public string AirlineId { get; set; }

        [RegularExpression(@"^([0-9]{1,2}[A-Za-z]{3})|([A-Za-z]{3}[0-9]{1,2})$", ErrorMessage = "Date must be in the format dMMM or DDMMM")]
        public DateTime? DepartureDate { get; set; }

        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Destination must be in the format XXX")]
        public string DestinationFrom { get; set; }

        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Destination must be in the format XXX")]
        public string DestinationTo { get; set; }
    }
}
