using Core.BoardingContext;
using Core.FlightContext.FlightInfo;
using Core.FlightContext.FlightInfo.Enums;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext;
using Core.SeatingContext.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.FlightContext
{
    public class Flight
    {
        public int Id { get; set; } 
        
        public ScheduledFlight ScheduledFlight { get; private set; }
        public string ScheduledFlightId { get; set; }
        
        public Aircraft Aircraft { get; private set; }
        public string AircraftId { get; set; }
        
        public Boarding Boarding { get; private set; }

        public int? DividerPlacedBehindRow { get; private set; }

        [Required]
        public DateTime DepartureDateTime { get; set; }

        [Required]
        public DateTime ArrivalDateTime { get; set; }        

        //ToDelete
        //public FlightTypeEnum FlightType { get; private set; } = FlightTypeEnum.Outbound;

        public FlightStatusEnum FlightStatus { get; private set; } = FlightStatusEnum.Closed;

        public int TotalBookedPassengers
        {
            get
            {
                return ListOfBookedPassengers.Count;
            }
            set { }
        }

        public int TotalCheckedBaggage
        {
            get
            {
                return ListOfCheckedBaggage.Count;
            }
            private set { }
        }

        public List<PassengerFlight> ListOfBookedPassengers { get; set; } = new List<PassengerFlight>(); 
        
        public List<FlightBaggage> ListOfCheckedBaggage { get; private set; } = new List<FlightBaggage>(); 

        public List<Seat> Seats { get; private set; } = new List<Seat>();

        public Flight()
        {
        }

        public Flight(
        int id,
        string scheduledFlightNumber,
        DateTime departureDateTime,
        DateTime arrivalDateTime)
        {
            Id = id;
            ScheduledFlightId = scheduledFlightNumber;
            DepartureDateTime = departureDateTime;
            ArrivalDateTime = arrivalDateTime;
        }        

        public List<Seat> InitializeSeats()
        {
            Seats = new List<Seat>();
            foreach (var specialSeats in Aircraft.SeatMap.FlightClassesSpecification)
            {
                foreach (var seatIdentifier in specialSeats.ExitRowSeats)
                {
                    Seats.Add(new Seat(Id, seatIdentifier, SeatTypeEnum.EmergencyExit, specialSeats.FlightClass));
                }

                foreach (var seatIdentifier in specialSeats.BassinetSeats)
                {
                    Seats.Add(new Seat(Id, seatIdentifier, SeatTypeEnum.BassinetSeat, specialSeats.FlightClass));
                }
            }

            foreach (var otherSeats in Aircraft.SeatMap.FlightClassesSpecification)
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
                                Seats.Add(new Seat(Id, seatIdentifier, SeatTypeEnum.Standard, otherSeats.FlightClass));
                            }
                        }
                    }
                }
            }
            return Seats;
        }
    }
}
