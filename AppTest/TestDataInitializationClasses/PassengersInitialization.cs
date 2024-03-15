using System.Diagnostics;
using Core.FlightContext;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TestProject.TestDataInitializationClasses
{
    public class PassengersInitialization
    {
        private readonly AppDbContext dbContext;
        private readonly Random random = new();

        public PassengersInitialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializePassengers()
        {
            var count = 0;
            var flights = dbContext.Flights.ToList();
            var paxList = dbContext.PassengerInfo.ToList();
            var ssrCodes = dbContext.SSRCodes.ToList();

            var bookingReferences = dbContext.BookingReferences.AsEnumerable()
                .Where(b => b.FlightItinerary.Any(k =>
                    flights.OfType<Flight>().Any(f => k.Value == f.DepartureDateTime && k.Key == f.ScheduledFlightId)))
                .ToList();

            foreach (var bookingReference in bookingReferences)
            {
                var flightsInPNR = flights.OfType<Flight>()
                    .Where(f => bookingReference.FlightItinerary.Any(g => g.Key == f.ScheduledFlightId) &&
                                bookingReference.FlightItinerary.Any(g => g.Value == f.DepartureDateTime))
                    .ToList();

                foreach (var passengerInfo in bookingReference.LinkedPassengers.ToList())
                {
                    var passenger = new Passenger();
                    passenger.MapFromPassengerInfo(passengerInfo);
                    dbContext.Passengers.Add(passenger);

                    foreach (var flight in flightsInPNR)
                    {
                        var bookedClass = passenger.BookedClass[flight.ScheduledFlightId];
                        var passengerFlight = new PassengerFlight(passenger.Id, flight.Id, bookedClass);

                        dbContext.PassengerFlight.Add(passengerFlight);

                        var passengerReservedSeats = paxList
                            .FirstOrDefault(f => f.Id == passengerInfo.Id)
                            ?.ReservedSeats;

                        if (passengerReservedSeats != null && passengerReservedSeats.Any())
                        {
                            var reservedSeatsValue = passengerReservedSeats
                                .FirstOrDefault(j => j.Key == flight.ScheduledFlightId)
                                .Value;

                            if (reservedSeatsValue != null)
                            {
                                var matchingSeat = dbContext.Seats.FirstOrDefault(f =>
                                    f.SeatNumber == reservedSeatsValue &&
                                    f.Flight.DepartureDateTime == flight.DepartureDateTime && f.FlightId == flight.Id);

                                if (matchingSeat != null)
                                {
                                    matchingSeat.Passenger = passenger;
                                }
                            }
                        }
                    }

                    var first = paxList.FirstOrDefault(f => f.Id == passengerInfo.Id);

                    if (first != null && first.BookedSSR.Any(m => m.Value != null))
                    {
                        foreach (var (key, values) in passengerInfo.BookedSSR)
                        {
                            foreach (var value in values)
                            {
                                var serviceRequest = value.Split('-', 2).Select(part => part.Trim()).ToArray();

                                if (passenger?.SpecialServiceRequests == null)
                                {
                                    if (passenger != null)
                                        passenger.SpecialServiceRequests = new List<SpecialServiceRequest>();
                                }

                                var ssrCode = ssrCodes.FirstOrDefault(s => s.Code == serviceRequest[0])?.Code;
                                var flight = flightsInPNR.FirstOrDefault(s => s.ScheduledFlightId == key);

                                if (ssrCode != null && flight != null)
                                {
                                    passenger?.SpecialServiceRequests.Add(new SpecialServiceRequest(ssrCode, flight.Id,
                                        passenger.Id, serviceRequest.Length > 1 ? serviceRequest[1] : null));
                                }
                            }
                        }
                    }

                    Trace.WriteLine($"Iteration {count++}");
                }

                foreach (var flight in flightsInPNR)
                {
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
                var passengerIds = passengerFlights.Where(pf => pf.FlightId == flight.Id)
                    .Select(pf => pf.PassengerId)
                    .ToList();

                var passengerList = dbContext.Passengers
                    .Where(p => p.AssignedSeats.Any() && p.TravelDocuments.Any() && passengerIds.Contains(p.Id))
                    .Include(passenger => passenger.TravelDocuments)
                    .ToList();

                var notAcceptedPassengers = passengerList.Where(p =>
                        passengerFlights.FirstOrDefault(pf => pf.PassengerId == p.Id)?.AcceptanceStatus ==
                        AcceptanceStatusEnum.NotAccepted)
                    .ToList();

                var i = 1;

                foreach (var passenger in notAcceptedPassengers)
                {
                    var hasPassengerTwoFlights = passengerFlights.Count(pf => pf.PassengerId == passenger.Id) == 2;
                    var passengerFlightsList = passengerFlights.Where(pf => pf.PassengerId == passenger.Id).ToList();

                    int randomValue;
                    if (notAcceptedPassengers.Any(p =>
                            passengerFlights.FirstOrDefault(f => f.FlightId == flight.Id && p.PNRId == passenger.PNRId)
                                ?.AcceptanceStatus == AcceptanceStatusEnum.Accepted))
                    {
                        randomValue = 60;
                    }
                    else
                    {
                        randomValue = random.Next(0, 101);
                    }

                    var passengerSeat = dbContext.Seats.FirstOrDefault(s =>
                        s.FlightId == flight.Id && s.PassengerId == passenger.Id);

                    if ((randomValue > 40 || hasPassengerTwoFlights && passengerSeat != null &&
                            passengerFlightsList.Any(p => p.AcceptanceStatus == AcceptanceStatusEnum.Accepted)))
                    {
                        var passengerFlight = passengerFlights.FirstOrDefault(pf =>
                            pf.PassengerId == passenger.Id && pf.FlightId == flight.Id);

                        if (passengerFlight != null)
                        {
                            passengerFlight.AcceptanceStatus = AcceptanceStatusEnum.Accepted;
                            passengerFlight.BoardingSequenceNumber = i;

                            if (passengerSeat != null) passengerSeat.SeatStatus = SeatStatusEnum.Occupied;

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
                        }
                        
                        var docsComment = predefinedComments.FirstOrDefault(pc => pc.Id == "Docs");
                        var exitComment = predefinedComments.FirstOrDefault(pc => pc.Id == "Exit");

                        if (passenger.TravelDocuments.All(p =>
                                countries.FirstOrDefault(c => c.Country2LetterCode == p.NationalityId)?.IsEUCountry !=
                                true) && docsComment != null)
                        {
                            var newComment = new Comment(passenger.Id, CommentTypeEnum.Gate, false, docsComment);

                            dbContext.Comments.Add(newComment);
                        }

                        if (passengerSeat is { SeatType: SeatTypeEnum.EmergencyExit } && exitComment != null)
                        {
                            var newComment = new Comment(passenger.Id, CommentTypeEnum.Gate, false, exitComment);

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