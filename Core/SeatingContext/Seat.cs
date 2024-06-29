using Core.FlightContext;
using Core.PassengerContext;
using Core.SeatingContext.Enums;

namespace Core.SeatingContext
{
    public class Seat
    {        
        public Guid Id { get; private set; }
        public BasePassengerOrItem PassengerOrItem  { get; set; }
        public Guid? PassengerOrItemId { get; set; }

        public Flight Flight { get; }
        public Guid FlightId { get; private set; }
        

        public string SeatNumber
        {
            get => $"{Row}{Letter}";

            private set 
            {
                if (!string.IsNullOrEmpty(value) && value.Length >= 2)
                {
                    Row = int.Parse(value[..^1]);
                    Letter = (SeatLetterEnum)Enum.Parse(typeof(SeatLetterEnum), value[^1..]);
                }
            }
        }

        public int Row { get; private set; }

        public SeatLetterEnum Letter { get; private set; }

        public SeatPositionEnum Position { get; }

        public SeatTypeEnum SeatType { get; private set; }

        public FlightClassEnum FlightClass { get; set; }    
        
        public SeatStatusEnum SeatStatus { get; set; } = SeatStatusEnum.Empty;

        public Seat(
            Guid flightId,
            string seatNumber,
            SeatTypeEnum seatType,
            FlightClassEnum flightClass 
            )
        {
            Id = Guid.NewGuid();
            FlightId = flightId;
            SeatNumber = seatNumber;
            SeatType = seatType;
            FlightClass = flightClass;
        }
    }
}
