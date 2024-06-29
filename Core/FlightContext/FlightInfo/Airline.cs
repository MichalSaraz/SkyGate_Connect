using System.ComponentModel.DataAnnotations;
using Core.PassengerContext.APIS;

namespace Core.FlightContext.FlightInfo
{
    public class Airline
    {
        [Key]
        public string CarrierCode { get; set; }

        public Country Country { get; set; }
        public string CountryId { get; set; }
                
        public int? AccountingCode { get; set; }
        
        [Required]
        public string Name { get; set; }       
        public string AirlinePrefix { get; private set; } = "000";  
        public List<Aircraft> Fleet { get; private set; } = new();
    }
}
