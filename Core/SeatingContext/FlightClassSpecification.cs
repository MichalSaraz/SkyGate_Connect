using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SeatingContext
{
    public class FlightClassSpecification
    {
        public int Id { get; private set; }

        public FlightClassEnum FlightClass { get; private set; }

        public List<string> SeatPositionsAvailable { get; private set; }

        public List<string> ExitRowSeats { get; private set; }

        public List<string> BassinetSeats { get; private set; }

        public List<string> NotExistingSeats { get; private set; }

        public List<int> RowRange { get; private set; }
    }
}
