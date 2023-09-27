using Core.PassengerContext.Regulatory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FlightContext.FlightInfo
{
    public class Airline
    {
        public int Id { get; private set; }

        [Required]
        [MinLength(2)]
        [MaxLength(2)]
        public string CarrierCode { get; private set; }

        public Country Country { get; private set; }
        public int CountryId { get; private set; }
                

        [Required]
        public string Name { get; private set; }       

        public int? AccountingCode { get; private set; }

        public int? AirlinePrefix { get; private set; }        
        

        public List<Aircraft> Fleet { get; private set; } = new List<Aircraft>();
    }
}
