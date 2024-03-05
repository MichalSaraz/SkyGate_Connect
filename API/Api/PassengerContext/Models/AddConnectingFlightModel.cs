using Core.SeatingContext.Enums;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace API.Api.PassengerContext.Models
{
    public class AddConnectingFlightModel
    {
        public string AirlineId { get; set; }
        public string FlightNumber { get; set; }

        [RegularExpression(@"^([0-9]{1,2}[A-Za-z]{3})|([A-Za-z]{3}[0-9]{1,2})$", ErrorMessage = "Date must be in the format dMMM or DDMMM")]
        public string DepartureDate { get; set; }

        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Destination must be in the format XXX")]
        public string DestinationFrom { get; set; }

        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Destination must be in the format XXX")]
        public string DestinationTo { get; set; }
        public FlightClassEnum FlightClass { get; set; }
    }
}
