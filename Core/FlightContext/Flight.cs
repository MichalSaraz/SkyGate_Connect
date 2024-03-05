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
    public class Flight : BaseFlight
    {
        public ScheduledFlight ScheduledFlight { get; private set; }
        public string ScheduledFlightId { get; set; }

        public Aircraft Aircraft { get; private set; }
        public string AircraftId { get; set; }
        
        public Boarding Boarding { get; private set; }

        public int? DividerPlacedBehindRow { get; private set; }         

        public FlightStatusEnum FlightStatus { get; private set; } = FlightStatusEnum.Closed;         

        public List<Seat> Seats { get; private set; } = new List<Seat>();

        public Flight() : base()
        {
        }

        public Flight(
            string scheduledFlightNumber,
            DateTime departureDateTime,
            string destinationFromId,
            string destinationToId,
            string airlineId,
            DateTime? arrivalDateTime) 
            : base(
                  destinationFromId,
                  destinationToId,
                  airlineId, 
                  departureDateTime,
                  arrivalDateTime)
        {
            ScheduledFlightId = scheduledFlightNumber;
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
