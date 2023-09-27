using Core.PassengerContext.Regulatory;
using Core.SeatingContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Core.FlightContext.FlightInfo
{
    public class Aircraft
    {
        public int Id { get; private set; }

        [NotMapped]
        public string AircraftRegistrationCode
        {
            get
            {
                return $"{Country.AircraftRegistrationPrefix}{RegistrationCode}";
            }
            private set { }
        }

        [MaxLength(5)]
        public string RegistrationCode { get; private set; }


        public Country Country { get; set; }
        public int CountryId { get; private set; }

        public AircraftType AircraftType { get; private set; }
        public int AircraftTypeId { get; private set; }

        public Airline Airline { get; private set; }
        public int AirlineId { get; private set; }

        public SeatMap SeatMap { get; private set; }
        public int SeatMapId { get; private set; }
        

        public int JumpSeatsAvailable { get; private set; } = 0;



        //public int NumberOfSeats
        //{
        //    get
        //    {
        //        return new int?[]
        //        {
        //        }
        //        .Where(capacity => capacity.HasValue)
        //        .Sum(capacity => capacity.Value);
        //    }
        //    private set { }
        //}
    }
}
