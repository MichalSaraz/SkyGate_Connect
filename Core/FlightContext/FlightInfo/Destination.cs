using Core.PassengerContext.APIS;

namespace Core.FlightContext.FlightInfo
{
    public class Destination
    {
        public string IATAAirportCode { get; set; }
        public string AirportName { get; set; }

        public Country Country { get; set; }
        public string CountryId { get; set; }

        public List<BaseFlight> Departures { get; set; }

        public List<BaseFlight> Arrivals { get; set; }
    }
}
