using System.ComponentModel.DataAnnotations;

namespace Core.PassengerContext.APIS
{
    public class Country
    {
        [Key]
        public string Country2LetterCode { get; set; }

        public string Country3LetterCode { get; set; }        

        public string CountryName { get; set; }   
        
        public string[] AircraftRegistrationPrefix { get; set; }

        public bool IsEEACountry { get; set; }
    }
}
