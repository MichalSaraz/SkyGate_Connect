using System.Diagnostics;
using Core.FlightContext;
using Core.FlightContext.JoinClasses;
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
            var paxList = dbContext.PassengerBookingDetails.ToList();
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

                foreach (var passengerBookingDetails in bookingReference.LinkedPassengers.ToList())
                {
                    BasePassengerOrItem? passengerOrItem;

                    if (passengerBookingDetails.FirstName == "CBBG")
                    {
                        passengerOrItem = new CabinBaggageRequiringSeat(passengerBookingDetails.FirstName,
                            passengerBookingDetails.LastName, passengerBookingDetails.Gender,
                            passengerBookingDetails.Id, null);

                        //passenger.MapFromPassengerBookingDetails(passengerBookingDetails);
                        dbContext.Passengers.Add(passengerOrItem);
                    }
                    else if (passengerBookingDetails.FirstName == "EXST")
                    {
                        passengerOrItem = new ExtraSeat(passengerBookingDetails.FirstName,
                            passengerBookingDetails.LastName, passengerBookingDetails.Gender,
                            passengerBookingDetails.Id);

                        //passenger.MapFromPassengerBookingDetails(passengerBookingDetails);
                        dbContext.Passengers.Add(passengerOrItem);
                    }
                    else if (passengerBookingDetails.Age < 2)
                    {
                        var associatedPassenger = bookingReference.LinkedPassengers.FirstOrDefault(p =>
                            p.Age >= 18 && p.AssociatedPassengerBookingDetailsId == null);

                        passengerOrItem = new Infant(associatedPassenger?.Id ?? Guid.Empty,
                            passengerBookingDetails.FirstName, passengerBookingDetails.LastName,
                            passengerBookingDetails.Gender, passengerBookingDetails.Id, 0);

                        if (associatedPassenger != null)
                            associatedPassenger.AssociatedPassengerBookingDetailsId = passengerOrItem.Id;

                        //passenger.MapFromPassengerBookingDetails(passengerBookingDetails);
                        dbContext.Passengers.Add(passengerOrItem);
                    }
                    else
                    {
                        var weight = (passengerBookingDetails.Age < 12) ? 35 :
                            (passengerBookingDetails.Gender == PaxGenderEnum.M) ? 88 : 70;

                        passengerOrItem = new Passenger(passengerBookingDetails.BaggageAllowance,
                            passengerBookingDetails.PriorityBoarding, passengerBookingDetails.FirstName,
                            passengerBookingDetails.LastName, passengerBookingDetails.Gender,
                            passengerBookingDetails.Id, weight);

                        if (passengerBookingDetails.AssociatedPassengerBookingDetailsId != null)
                            ((Passenger)passengerOrItem).InfantId =
                                passengerBookingDetails.AssociatedPassengerBookingDetailsId;

                        //passenger.MapFromPassengerBookingDetails(passengerBookingDetails);
                        dbContext.Passengers.Add(passengerOrItem);
                    }

                    foreach (var flight in flightsInPNR)
                    {
                        var bookedClass = passengerBookingDetails.BookedClass[flight.ScheduledFlightId];
                        var passengerFlight = new PassengerFlight(passengerOrItem.Id, flight.Id, bookedClass);

                        dbContext.PassengerFlight.Add(passengerFlight);

                        var passengerReservedSeats = paxList.FirstOrDefault(f => f.Id == passengerBookingDetails.Id)
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
                                    matchingSeat.PassengerOrItem = passengerOrItem;
                                }
                            }
                        }
                    }

                    var first = paxList.FirstOrDefault(f => f.Id == passengerBookingDetails.Id);

                    if (first != null && first.BookedSSR.Any(m => m.Value != null))
                    {
                        foreach (var (key, values) in passengerBookingDetails.BookedSSR)
                        {
                            foreach (var value in values)
                            {
                                var serviceRequest = value.Split('-', 2).Select(part => part.Trim()).ToArray();

                                if ((passengerOrItem as Passenger)?.SpecialServiceRequests == null)
                                {
                                    if (passengerOrItem != null)
                                        ((Passenger)passengerOrItem).SpecialServiceRequests =
                                            new List<SpecialServiceRequest>();
                                }

                                var ssrCode = ssrCodes.FirstOrDefault(s => s.Code == serviceRequest[0])?.Code;
                                var flight = flightsInPNR.FirstOrDefault(s => s.ScheduledFlightId == key);

                                if (ssrCode != null && flight != null && (passengerOrItem as Passenger) != null)
                                {
                                    (passengerOrItem as Passenger)?.SpecialServiceRequests.Add(
                                        new SpecialServiceRequest(ssrCode, flight.Id, passengerOrItem.Id,
                                            serviceRequest.Length > 1 ? serviceRequest[1] : null));
                                }
                            }
                        }
                    }

                    Trace.WriteLine($"Iteration {count++}");
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
                    .Select(pf => pf.PassengerOrItemId)
                    .ToList();

                var passengerList = dbContext.Passengers
                    .Where(p => p.AssignedSeats.Any() && p.TravelDocuments.Any() && passengerIds.Contains(p.Id))
                    .Include(p => p.TravelDocuments)
                    .Include(p => p.BookingDetails)
                    .ToList();

                var notAcceptedPassengers = passengerList.Where(p =>
                        passengerFlights.FirstOrDefault(pf => pf.PassengerOrItemId == p.Id)?.AcceptanceStatus ==
                        AcceptanceStatusEnum.NotAccepted)
                    .ToList();

                var i = 1;

                foreach (var passenger in notAcceptedPassengers)
                {
                    var hasPassengerTwoFlights =
                        passengerFlights.Count(pf => pf.PassengerOrItemId == passenger.Id) == 2;
                    var passengerFlightsList =
                        passengerFlights.Where(pf => pf.PassengerOrItemId == passenger.Id).ToList();

                    var isAnyPassengerAccepted = notAcceptedPassengers.Any(p => passengerFlights.FirstOrDefault(f =>
                            f.FlightId == flight.Id && p.BookingDetails.PNRId == passenger.BookingDetails.PNRId)
                        ?.AcceptanceStatus == AcceptanceStatusEnum.Accepted);

                    int randomValue;
                    if (isAnyPassengerAccepted)
                    {
                        randomValue = 60;
                    }
                    else
                    {
                        randomValue = random.Next(0, 101);
                    }

                    var passengerSeat = dbContext.Seats.FirstOrDefault(s =>
                        s.FlightId == flight.Id && s.PassengerOrItemId == passenger.Id);

                    if ((randomValue > 40 || hasPassengerTwoFlights && passengerSeat != null &&
                            passengerFlightsList.Any(p => p.AcceptanceStatus == AcceptanceStatusEnum.Accepted)))
                    {
                        var passengerFlight = passengerFlights.FirstOrDefault(pf =>
                            pf.PassengerOrItemId == passenger.Id && pf.FlightId == flight.Id);

                        if (passengerFlight != null)
                        {
                            passengerFlight.AcceptanceStatus = AcceptanceStatusEnum.Accepted;
                            passengerFlight.BoardingSequenceNumber = i;

                            if (passengerSeat != null) passengerSeat.SeatStatus = SeatStatusEnum.Occupied;

                            if (((Passenger)passenger).PriorityBoarding)
                            {
                                passengerFlight.BoardingZone = BoardingZoneEnum.A;
                            }
                            else if (((Passenger)passenger).BaggageAllowance > 0)
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
                                countries.FirstOrDefault(c => c.Country2LetterCode == p.NationalityId)?.IsEEACountry !=
                                true) && docsComment != null)
                        {
                            var newComment = new Comment(passenger.Id, docsComment.Id, docsComment.Text);

                            foreach (var iteratedPassengerFlight in passenger.Flights)
                            {
                                var flightComment = new FlightComment(newComment.Id, iteratedPassengerFlight.FlightId);
                                dbContext.FlightComment.Add(flightComment);
                            }

                            dbContext.Comments.Add(newComment);
                        }

                        if (passengerSeat is { SeatType: SeatTypeEnum.EmergencyExit } && exitComment != null)
                        {
                            var newComment = new Comment(passenger.Id, exitComment.Id, exitComment.Text);

                            foreach (var iteratedPassengerFlight in passenger.Flights)
                            {
                                var flightComment = new FlightComment(newComment.Id, iteratedPassengerFlight.FlightId);
                                dbContext.FlightComment.Add(flightComment);
                            }

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