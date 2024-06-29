using Core.SeatingContext;
using System.ComponentModel.DataAnnotations;
using Core.PassengerContext.APIS;

namespace Core.FlightContext.FlightInfo
{
    public class Aircraft
    {
        [Key]             
        public string RegistrationCode { get; set; }

        public List<Flight> Flights { get; private set; } = new();

        public Country Country { get; set; }
        public string CountryId { get; set; }

        public AircraftType AircraftType { get; set; }
        public string AircraftTypeId { get; set; }

        public Airline Airline { get; set; }
        public string AirlineId { get; set; }

        public SeatMap SeatMap { get; set; }
        public string SeatMapId { get; set; }        

        public int JumpSeatsAvailable { get; set; }
    }
}
