using Core.FlightContext;
using Core.FlightContext.FlightInfo;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Core.SeatingContext.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SeatingContext
{
    public class Seat
    {        
        public Guid Id { get; private set; }
        public Passenger Passenger  { get; set; }
        public Guid? PassengerId { get; private set; }

        public Flight Flight { get; private set; }
        public int FlightId { get; private set; }
        

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

        public FlightClassEnum FlightClass { get; set; }    
        
        public SeatStatusEnum SeatStatus { get; set; } = SeatStatusEnum.Empty; 
        

        // public bool IsChargeable { get; private set; }


        public Seat(
            int flightId,
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
