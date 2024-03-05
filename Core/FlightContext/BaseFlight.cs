using Core.FlightContext.FlightInfo;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext.JoinClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FlightContext
{
    public abstract class BaseFlight
    {
        public int Id { get; set; }        

        public Destination DestinationFrom { get; private set; }
        public string DestinationFromId { get; set; }

        public Destination DestinationTo { get; private set; }
        public string DestinationToId { get; set; }

        public Airline Airline { get; private set; }
        public string AirlineId { get; set; }

        [Required]
        public DateTime DepartureDateTime { get; set; }

        public DateTime? ArrivalDateTime { get; set; }

        public int TotalBookedPassengers
        {
            get
            {
                return ListOfBookedPassengers.Count;
            }
            set { }
        }

        public int TotalCheckedBaggage
        {
            get
            {
                return ListOfCheckedBaggage.Count;
            }
            private set { }
        }

        public List<PassengerFlight> ListOfBookedPassengers { get; set; } = new List<PassengerFlight>();

        public List<FlightBaggage> ListOfCheckedBaggage { get; private set; } = new List<FlightBaggage>();

        protected BaseFlight(
            string destinationFromId, 
            string destinationToId, 
            string airlineId, 
            DateTime departureDateTime,
            DateTime? arrivalDateTime)
        {
            DestinationFromId = destinationFromId;
            DestinationToId = destinationToId;
            AirlineId = airlineId;
            DepartureDateTime = departureDateTime;
            ArrivalDateTime = arrivalDateTime;

        }

        protected BaseFlight()
        {
        }
    }
}
