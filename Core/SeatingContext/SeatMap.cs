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
        public string Id { get; private set; }

        //public string SeatMapId
        //{
        //    get
        //    {
        //        var classCounts = Seats
        //            .GroupBy(s => s.FlightClass)
        //            .Select(group => new
        //            {
        //                ClassName = group.Key,
        //                SeatCount = group.Count()
        //            })
        //            .Where(cls => cls.SeatCount > 0)
        //            .Select(cls => $"{cls.SeatCount}");

        //        return $"{Airline.CarrierCode}-" +
        //               $"{AircraftType.AircraftTypeIATACode}-" +
        //               $"{string.Join("/", classCounts)}";
        //    }
        //    private set { }
        //}

        //public int[] RowsChargeable { get; private set; } = null;


        public List<FlightClassSpecification> FlightClassesSpecification { get; private set; } = new List<FlightClassSpecification>();

        public SeatMap(string id, List<FlightClassSpecification> flightClassesSpecification)
        {
            Id = id;
            FlightClassesSpecification = flightClassesSpecification;
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