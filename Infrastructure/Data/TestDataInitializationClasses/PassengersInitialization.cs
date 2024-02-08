using Core.PassengerContext.Booking;
using Core.PassengerContext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.FlightContext;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext.Enums;
using Core.PassengerContext.Booking.Enums;

namespace Infrastructure.Data.TestDataInitializationClasses
{
    public class PassengersInitialization
    {
        private readonly AppDbContext dbContext;
        private readonly Random random = new Random();

        public PassengersInitialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializePassengers()
        {
            int pocet = 0;
            var flights = dbContext.Flights.ToList();
            var paxList = dbContext.PassengerInfo.ToList();
            var ssrCodes = dbContext.SSRCodes.ToList();

            var bookingReferences = dbContext.BookingReferences
                .AsEnumerable()
                .Where(b => b.FlightItinerary.Any(k => flights.Any(f => k.Value == f.DepartureDateTime && k.Key == f.ScheduledFlightId))).ToList();

            foreach (var bookingReference in bookingReferences)
            {
                var flightsInPNR = flights
                    .Where(f => bookingReference.FlightItinerary
                    .Any(g => g.Key == f.ScheduledFlightId) && bookingReference.FlightItinerary
                    .Any(g => g.Value == f.DepartureDateTime))
                    .ToList();

                // Přidejte cestující k letu
                foreach (var passengerInfo in bookingReference.LinkedPassengers.ToList())
                {
                    Passenger passenger = new Passenger();
                    passenger.MapFromPassengerInfo(passengerInfo);
                    dbContext.Passengers.Add(passenger);

                    foreach (var flight in flightsInPNR)
                    {
                        PassengerFlight passengerFlight = new PassengerFlight
                        {
                            Passenger = passenger,
                            Flight = flight
                        };
                        dbContext.PassengerFlight.Add(passengerFlight);
                        //dbContext.Passengers.FirstOrDefault(p => p.Id == passenger.Id).Flights.Add(passengerFlight);                        

                        var passengerReservedSeats = paxList.FirstOrDefault(f => f.Id == passengerInfo.Id)?.ReservedSeats;

                        if (passengerReservedSeats != null && passengerReservedSeats.Any())
                        {
                            var reservedSeatsValue = passengerReservedSeats
                                .FirstOrDefault(j => j.Key == flight.ScheduledFlightId).Value;

                            if (reservedSeatsValue != null)
                            {
                                var matchingSeat = dbContext.Seats.FirstOrDefault(f =>
                                    f.SeatNumber == reservedSeatsValue &&
                                    f.Flight.DepartureDateTime == flight.DepartureDateTime &&
                                    f.FlightId == flight.Id);

                                if (matchingSeat != null)
                                {
                                    matchingSeat.Passenger = passenger;
                                }
                            }
                        }
                    }

                    if (paxList.FirstOrDefault(f => f.Id == passengerInfo.Id).BookedSSR.Any(m => m.Value != null))
                    {
                        foreach (var keyValuePair in passengerInfo.BookedSSR)
                        {
                            var values = keyValuePair.Value; // Seznam hodnot pro aktuální klíč

                            foreach (var value in values)
                            {
                                var serviceRequest = value.Split('-', 2)
                                    .Select(part => part.Trim())
                                    .ToArray();

                                if (passenger?.SpecialServiceRequests == null)
                                {
                                    passenger.SpecialServiceRequests = new List<SpecialServiceRequest>();
                                }

                                passenger?.SpecialServiceRequests.Add(
                                    new SpecialServiceRequest(
                                        ssrCodes.FirstOrDefault(s => s.Code == serviceRequest[0]),
                                        flightsInPNR.FirstOrDefault(s => s.ScheduledFlightId == keyValuePair.Key),
                                        passenger,
                                        serviceRequest.Length > 1 ? serviceRequest[1] : null)
                                    );
                            }
                        }
                    }
                    Trace.WriteLine($"Iteration {pocet++}");
                }

                foreach (var flight in flightsInPNR)
                {
                    dbContext.Flights.FirstOrDefault(f => f.Id == flight.Id).TotalBookedPassengers += bookingReference.LinkedPassengers.Count;
                }
            }
            dbContext.SaveChanges();
        }

        public void AcceptingRandomCustomers()
        {
            var flights = dbContext.Flights.ToList();
            var countries = dbContext.Countries.ToList();
            var predefinedComments = dbContext.PredefinedComments.ToList();
            var passengerFlights = dbContext.PassengerFlight.ToList();

            foreach (var flight in flights)
            {
                var passengerIds = passengerFlights
                    .Where(pf => pf.FlightId == flight.Id)
                    .Select(pf => pf.PassengerId)
                    .ToList();

                var passengerList = dbContext.Passengers
                    .Where(p =>
                        p.AssignedSeats.Any() &&
                        p.TravelDocuments.Any() &&
                        passengerIds.Contains(p.Id))
                    .ToList();

                //create list of passengers with one flight with not accepted status
                var notAcceptedPassengers = passengerList
                    .Where(p => passengerFlights
                        .FirstOrDefault(pf => pf.PassengerId == p.Id)
                        .AcceptanceStatus == AcceptanceStatusEnum.NotAccepted)
                    .ToList();

                int randomValue = 0;
                int i = 1;

                foreach (var passenger in notAcceptedPassengers)
                {
                    var hasPassengerTwoFlights = passengerFlights.Where(pf => pf.PassengerId == passenger.Id).Count() == 2;
                    var passengerFlightsList = passengerFlights.Where(pf => pf.PassengerId == passenger.Id).ToList();

                    if (notAcceptedPassengers.Any(p =>
                        passengerFlights.FirstOrDefault(f =>
                            f.FlightId == flight.Id &&
                            p.PNRId == passenger.PNRId)?.AcceptanceStatus == AcceptanceStatusEnum.Accepted
                            )
                        )
                    {
                        randomValue = 60;
                    }
                    else
                    {
                        randomValue = random.Next(0, 101);
                    }

                    var passengerSeat = dbContext.Seats.FirstOrDefault(s =>
                            s.FlightId == flight.Id && s.PassengerId == passenger.Id);

                    if ((randomValue > 40 || hasPassengerTwoFlights &&
                        passengerFlightsList.Any(p => p.AcceptanceStatus == AcceptanceStatusEnum.Accepted)) && passengerSeat != null)
                    {
                        var passengerFlight = passengerFlights
                            .FirstOrDefault(pf => pf.PassengerId == passenger.Id && pf.FlightId == flight.Id);

                        passengerFlight.AcceptanceStatus = AcceptanceStatusEnum.Accepted;
                        passengerFlight.BoardingSequenceNumber = i;
                        passengerSeat.SeatStatus = SeatStatusEnum.Occupied;

                        if (passenger.PriorityBoarding)
                        {
                            passengerFlight.BoardingZone = BoardingZoneEnum.A;
                        }
                        else if (passenger.BaggageAllowance > 0)
                        {
                            passengerFlight.BoardingZone = BoardingZoneEnum.B;
                        }
                        else
                        {
                            passengerFlight.BoardingZone = BoardingZoneEnum.C;
                        }

                        if (!passenger.TravelDocuments.Any(p =>
                            countries.FirstOrDefault(c =>
                                c.Country2LetterCode == p.NationalityId).IsEUCountry)
                            )
                        {
                            var newComment = new Comment(
                                passenger.Id,
                                CommentTypeEnum.Gate,
                                false,
                                predefinedComments.FirstOrDefault(pc => pc.Id == "Docs")
                            );

                            dbContext.Comments.Add(newComment);
                        }

                        if (passengerSeat.SeatType == SeatTypeEnum.EmergencyExit)
                        {
                            var newComment = new Comment(
                               passenger.Id,
                               CommentTypeEnum.Gate,
                               false,
                               predefinedComments.FirstOrDefault(pc => pc.Id == "Exit")
                            );

                            dbContext.Comments.Add(newComment);
                        }
                        Trace.WriteLine($"Let {flights.IndexOf(flight)}");
                        i++;
                    }
                }
            }
            dbContext.SaveChanges();
        }
    }
}
