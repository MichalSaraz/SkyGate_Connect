using Core.FlightContext.FlightInfo;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext.JoinClasses;
using System.ComponentModel.DataAnnotations;

namespace Core.FlightContext
{
    public abstract class BaseFlight
    {
        public int Id { get; private set; }        

        public Destination DestinationFrom { get; private set; }
        public string DestinationFromId { get; private set; }

        public Destination DestinationTo { get; private set; }
        public string DestinationToId { get; private set; }

        public Airline Airline { get; private set; }
        public string AirlineId { get; private set; }

        [Required]
        public DateTime DepartureDateTime { get; private set; }

        public DateTime? ArrivalDateTime { get; private set; }

        public List<PassengerFlight> ListOfBookedPassengers { get; set; } = new();

        public List<FlightBaggage> ListOfCheckedBaggage { get; set; } = new();

        protected BaseFlight(
            DateTime departureDateTime,
            DateTime? arrivalDateTime,
            string destinationFromId, 
            string destinationToId, 
            string airlineId)
        {
            DepartureDateTime = departureDateTime;
            ArrivalDateTime = arrivalDateTime;
            DestinationFromId = destinationFromId;
            DestinationToId = destinationToId;
            AirlineId = airlineId;

        }
    }
}
