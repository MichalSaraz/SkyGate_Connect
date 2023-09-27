using Core.BaggageContext;
using Core.BoardingContext;
using Core.FlightContext.FlightInfo;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext.JoinClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FlightContext
{
    public class Flight
    {
        public int Id { get; private set; }
        public string FlightNumber 
        { 
            get
            {
                return $"{Airline.CarrierCode}{FlightIdentifier}";
            }
            private set { }        
        }

        [Required]
        public Destination DepartureAirport { get; private set; }
        public int DepartureAirportId { get; private set; }

        [Required]
        public Destination ArrivalAirport { get; private set; }
        public int ArrivalAirportId { get; private set; }

        [Required]
        public Airline Airline { get; private set; }
        public int AirlineId { get; private set; }

        [Required]
        public Aircraft Aircraft { get; private set; }
        public int AircraftId { get; private set; }

        [Required]
        public Boarding Boarding { get; private set; }


        [Required]
        [Range(0001,2999)]
        public int FlightIdentifier { get; private set; }

        [Required]
        public DateTime DepartureDateTime { get; private set; }

        [Required]
        public DateTime ArrivalDateTime { get; private set; }        

        public FlightTypeEnum FlightType { get; private set; } = FlightTypeEnum.Outbound;

        public FlightStatusEnum FlightStatus { get; private set; } = FlightStatusEnum.Closed;


        public int TotalBookedPassengers
        {
            get
            {
                return ListOfBookedPassengers.Count;
            }
            private set { }
        }

        public int TotalCheckedBaggage
        {
            get
            {
                return ListOfCheckedBaggage.Count;
            }
            private set { }
        }


        public List<PassengerFlight> ListOfBookedPassengers { get; private set; } = new List<PassengerFlight>(); 
        
        public List<FlightBaggage> ListOfCheckedBaggage { get; private set; } = new List<FlightBaggage>();        
        
        public List<Codeshare> Codeshare { get; private set; } = new List<Codeshare>();
    }
}
