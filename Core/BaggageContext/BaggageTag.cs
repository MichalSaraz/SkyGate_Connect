using Core.FlightContext.FlightInfo;
using Core.PassengerContext.Regulatory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BaggageContext
{
    public class BaggageTag
    {
        public int Id { get; private set; }

        public string TagNumber
        {
            get
            {
                return $"{LeadingDigit}{Airline.AirlinePrefix}{Number}";
            }
            private set { }
        }

        public Airline Airline { get; private set; }
        public string AirlineId { get; private set; }


        public int LeadingDigit { get; private set; } = 0;   
        
        public long Number { get; private set; }
    }
}
