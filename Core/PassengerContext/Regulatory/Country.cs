using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext.Regulatory
{
    public class Country
    {
        [Key]
        public string Country2LetterCode { get; private set; }

        public string Country3LetterCode { get; private set; }        

        public string CountryName { get; private set; }   
        
        public string[] AircraftRegistrationPrefix { get; private set; }
    }
}
