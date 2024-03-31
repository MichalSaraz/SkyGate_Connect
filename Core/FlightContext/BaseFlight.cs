using Core.FlightContext.FlightInfo;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext.JoinClasses;
using System.ComponentModel.DataAnnotations;

namespace Core.FlightContext
{
    public abstract class BaseFlight
    {
        public int Id { get; protected set; }        

        public Destination DestinationFrom { get; protected set; }
        public string DestinationFromId { get; protected set; }

        public Destination DestinationTo { get; protected set; }
        public string DestinationToId { get; protected set; }

        public Airline Airline { get; protected set; }
        public string AirlineId { get; protected set; }

        [Required]
        public DateTime DepartureDateTime { get; set; }

        public DateTime? ArrivalDateTime { get; protected set; }

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
