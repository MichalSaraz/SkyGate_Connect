using Core.SeatingContext.Enums;

namespace Core.SeatingContext
{
    public abstract class FlightClassSpecification
    {
        public FlightClassEnum FlightClass { get; }
        public List<string> SeatPositionsAvailable { get; }
        public List<string> WindowPositions { get; }
        public List<string> AislePositions { get; }
        public List<string> MiddlePositions { get; }
        public List<string> ExitRowSeats { get; }
        public List<string> BassinetSeats { get; }
        public List<string> NotExistingSeats { get; }
        public List<int> RowRange { get; }
    }
}
