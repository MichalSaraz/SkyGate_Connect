using System.ComponentModel.DataAnnotations;
using Core.PassengerContext.APIS;

namespace Core.FlightContext.FlightInfo
{
    public class Airline
    {   
        public string CarrierCode { get; set; }

        public Country Country { get; private set; }
        public string CountryId { get; private set; }
                
        public int? AccountingCode { get; private set; }
        
        [Required]
        public string Name { get; private set; }       
        public string AirlinePrefix { get; private set; } = "000";  
        public List<Aircraft> Fleet { get; private set; } = new();
    }
}
