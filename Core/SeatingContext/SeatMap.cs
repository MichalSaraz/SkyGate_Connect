using Core.FlightContext.FlightInfo;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SeatingContext
{
    public class SeatMap
    {
        public int Id { get; private set; }

        public string SeatMapId
        {
            get
            {
                var classCounts = Seats
                    .GroupBy(s => s.FlightClass)
                    .Select(group => new
                    {
                        ClassName = group.Key,
                        SeatCount = group.Count()
                    })
                    .Where(cls => cls.SeatCount > 0)
                    .Select(cls => $"{cls.SeatCount}");

                return $"{Airline.CarrierCode}-" +
                       $"{AircraftType.AircraftTypeIATACode}-" +
                       $"{string.Join("/", classCounts)}";
            }
            private set { }
        }


        public Airline Airline { get; private set; }
        public int AirlineId { get; private set; }

        public AircraftType AircraftType { get; private set; }
        public int AircraftTypeId { get; private set; }


        //public int[] RowsChargeable { get; private set; } = null;


        public List<FlightClassSpecification> FlightClassesSpecification { get; private set; } = new List<FlightClassSpecification>();

        public List<Seat> Seats { get; private set; } = new List<Seat>();        


        public SeatMap()            
        {
            InitializeSeats();
        }

        private void InitializeSeats()
        {
            foreach (var specialSeats in FlightClassesSpecification)
            {
                foreach (var seatIdentifier in specialSeats.ExitRowSeats)
                {                    
                    Seats.Add(new Seat(seatIdentifier, SeatTypeEnum.EmergencyExit, specialSeats.FlightClass));
                }

                foreach (var seatIdentifier in specialSeats.BassinetSeats)
                {
                    Seats.Add(new Seat(seatIdentifier, SeatTypeEnum.BassinetSeat, specialSeats.FlightClass));
                }
            }

            foreach (var otherSeats in FlightClassesSpecification)
            {
                for (int row = otherSeats.RowRange[0]; row <= otherSeats.RowRange[1]; row++)
                {
                    foreach (var position in otherSeats.SeatPositionsAvailable)
                    {
                        string seatIdentifier = $"{row}{position}";

                        if (!otherSeats.NotExistingSeats.Contains(seatIdentifier))
                        {
                            if (!Seats.Any(s => s.SeatNumber == seatIdentifier))
                            {
                                Seats.Add(new Seat(seatIdentifier, SeatTypeEnum.Standard, otherSeats.FlightClass));
                            }
                        }
                    }
                }
            }
        }

        //private void InitializeSeats()
        //{
        //    for (int i = 0; i < RowRange.Length; i++)
        //    {
        //        int seatCount = (int)SeatPositionEnum.K - (int)SeatPositionEnum.A + 1;

        //        for (int j = 0; j < seatCount; j++)
        //        {
        //            string seatIdentifier = $"{RowRange[i]}{(SeatPositionEnum)(j + (int)SeatPositionEnum.A)}";

        //            if (NotExistingSeats != null && NotExistingSeats.Contains(seatIdentifier))
        //            {
        //                continue;
        //            }
        //            else if (ExitRowSeats != null && ExitRowSeats.Contains(seatIdentifier))
        //            {
        //                Seats.Add(new Seat(RowRange[i], (SeatPositionEnum)(j + 1), FlightClass, SeatTypeEnum.EmergencyExit));
        //            }
        //            else if (BassinetSeats != null && BassinetSeats.Contains(seatIdentifier))
        //            {
        //                Seats.Add(new Seat(RowRange[i], (SeatPositionEnum)(j + 1), FlightClass, SeatTypeEnum.BassinetSeat));
        //            }
        //            else
        //            {
        //                Seats.Add(new Seat(RowRange[i], (SeatPositionEnum)(j + 1), FlightClass, SeatTypeEnum.Standard));
        //            }
        //        }
        //    }
        //}
    }
}