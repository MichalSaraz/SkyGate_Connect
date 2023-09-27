using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FlightContext.FlightInfo
{
    public class Codeshare
    {
        public int Id { get; private set; }

        public string CodeshareFlightNumber
        {
            get
            {
                return $"{CodeshareAirline.CarrierCode}{CodeshareFlightIdentifier}";
            }
            private set { }
        }

        [Required]
        public Airline CodeshareAirline { get; private set; }
        public int CodeshareAirlineId { get; private set; }

        [Range(3000, 9999)]
        public int CodeshareFlightIdentifier { get; private set; }
    }
}
