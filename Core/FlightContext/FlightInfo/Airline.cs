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
        public string CarrierCode { get; private set; }

        public Country Country { get; private set; }
        public string CountryId { get; private set; }
                

        [Required]
        public string Name { get; private set; }       

        public int? AccountingCode { get; private set; }

        public int? AirlinePrefix { get; private set; }        
        

        public List<Aircraft> Fleet { get; private set; } = new List<Aircraft>();
    }
}
