using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class FlightBaggageDto
    {
        public FlightOverviewDto Flight { get; set; }
        public string BaggageType { get; set; }
    }
}
