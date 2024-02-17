using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class FlightDetailsDto : FlightOverviewDto
    {        
        public string[] CodeShare { get; set; }        
        public string Airline { get; set; }
        public int TotalBookedPassengers { get; set; }
    }
}
