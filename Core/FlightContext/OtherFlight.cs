using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FlightContext
{
    public class OtherFlight : BaseFlight
    {
        public string FlightNumber { get; set; }

        public OtherFlight( 
            string flightNumber,
            string destinationFromId,
            string destinationToId,
            string airlineId,
            DateTime departureDateTime,
            DateTime? arrivalDateTime) 
            : base(
                  destinationFromId, 
                  destinationToId, 
                  airlineId, 
                  departureDateTime,
                  arrivalDateTime)
        {
            FlightNumber = flightNumber;
        }
    }
}
