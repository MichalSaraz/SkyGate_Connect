using Core.SeatingContext;
using System.ComponentModel.DataAnnotations;
using Core.PassengerContext.APIS;

namespace Core.FlightContext.FlightInfo
{
    public class Aircraft
    {
        [Key]             
        public string RegistrationCode { get; private set; }

        public List<Flight> Flights { get; private set; } = new();

        public Country Country { get; private set; }
        public string CountryId { get; private set; }

        public AircraftType AircraftType { get; private set; }
        public string AircraftTypeId { get; private set; }

        public Airline Airline { get; private set; }
        public string AirlineId { get; private set; }

        public SeatMap SeatMap { get; private set; }
        public string SeatMapId { get; private set; }        

        public int JumpSeatsAvailable { get; private set; }
    }
}
