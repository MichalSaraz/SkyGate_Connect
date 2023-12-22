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

namespace Infrastructure.Data
{
    public class PassengersInitialization
    {
        private readonly AppDbContext dbContext;

        public PassengersInitialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializePassengers()
        {
            int pocet = 0;
            var flights = dbContext.Flights.ToList();
            var paxList = dbContext.PassengerInfos.ToList();
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
    }
}
