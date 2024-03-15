using Core.PassengerContext.APIS;

namespace Core.FlightContext.FlightInfo
{
    public class Destination
    {
        public string IATAAirportCode { get; private set; }
        public string AirportName { get; private set; }

        public Country Country { get; private set; }
        public string CountryId { get; private set; }

        public List<BaseFlight> Departures { get; private set; }

        public List<BaseFlight> Arrivals { get; private set; }
    }
}
