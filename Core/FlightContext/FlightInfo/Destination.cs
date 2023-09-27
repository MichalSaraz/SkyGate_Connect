using Core.PassengerContext.Regulatory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FlightContext.FlightInfo
{
    public class Destination
    {
        public int Id { get; private set; }

        public string IATAAirportCode { get; private set; }
        public string AirportName { get; private set; }

        public Country Country { get; private set; }
        public int CountryId { get; private set; }

        public List<Flight> DepartingFlights { get; private set; }
    }
}
