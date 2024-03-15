using System.ComponentModel.DataAnnotations;

namespace Core.PassengerContext.APIS
{
    public class Country
    {
        [Key]
        public string Country2LetterCode { get; private set; }

        public string Country3LetterCode { get; private set; }        

        public string CountryName { get; private set; }   
        
        public string[] AircraftRegistrationPrefix { get; private set; }

        public bool IsEUCountry { get; private set; }
    }
}
