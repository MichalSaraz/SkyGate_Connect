using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class FlightOverviewDto
    {
        public int Id { get; set; }
        public string ScheduledFlight { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public string DestinationFrom { get; set; }
        public string DestinationTo { get; set; }
    }
}
