using Core.FlightContext;
using Core.FlightContext.FlightInfo;
using Core.PassengerContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SeatingContext
{
    public class Seat
    {
        public int Id { get; private set; }

        public Passenger Passenger { get; private set; }
        public int PassengerId { get; private set; }
        

        public string SeatNumber
        {
            get { return $"{Row}{Position}"; }

            private set 
            {
                if (!string.IsNullOrEmpty(value) && value.Length >= 2)
                {
                    Row = int.Parse(value.Substring(0, value.Length - 1));
                    Position = (SeatPositionEnum)Enum.Parse(typeof(SeatPositionEnum), value.Substring(value.Length - 1));
                }
            }
        }

        public int Row { get; private set; }

        public SeatPositionEnum Position { get; private set; }

        public SeatTypeEnum SeatType { get; private set; }

        public FlightClassEnum FlightClass { get; private set; }    
        
        public SeatStatusEnum SeatStatus { get; private set; } = SeatStatusEnum.Empty; 
        

        // public bool IsChargeable { get; private set; }


        public Seat(string seatNumber,
            SeatTypeEnum seatType,
            FlightClassEnum flightClass 
            )
        {
            SeatNumber = seatNumber;
            SeatType = seatType;
            FlightClass = flightClass;
        }
    }
}
