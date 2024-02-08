using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.SeatingContext.Enums;

namespace Core.SeatingContext
{
    public class FlightClassSpecification
    {
        public FlightClassEnum FlightClass { get; set; }
        public List<string> SeatPositionsAvailable { get; set; }
        public List<string> ExitRowSeats { get; set; }
        public List<string> BassinetSeats { get; set; }
        public List<string> NotExistingSeats { get; set; }
        public List<int> RowRange { get; set; }
    }
}
