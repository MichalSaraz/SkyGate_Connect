using Core.PassengerContext.Booking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnassignedBookingItineraryInitialization
    {
        private readonly AppDbContext dbContext;

        public UnassignedBookingItineraryInitialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializeItinerary()
        {
            var flights = dbContext.Flights.ToList();
            Dictionary<int, DateTime> notLoadedFlights = new Dictionary<int, DateTime>();
            Dictionary<int, int> flightCounts = new Dictionary<int, int>();

            for (int i = 0; i < flights.Count; i++)
            {
                var flight = flights[i].Id;
                var dateForNotLoadedFlights = flights[i].DepartureDateTime.AddDays(8);
                notLoadedFlights.Add(flight, dateForNotLoadedFlights);
                flightCounts.Add(flight, 0);
            };

            int pocet = 0;

            var PNRWithoutItinerary = dbContext.BookingReferences.Where(b => b.FlightItinerary.Count == 0).ToList();

            foreach (var unassignedBooking in PNRWithoutItinerary)
            {
                unassignedBooking.LinkedPassengers = dbContext.PassengerInfos
                    .Where(p => p.PNRId == unassignedBooking.PNR).ToList();
                var foundFlight = FindFlightInformation(notLoadedFlights, unassignedBooking.LinkedPassengers.Count, flightCounts);
                var flightNumber = flights.SingleOrDefault(f => f.Id == foundFlight.Key).ScheduledFlightId;
                unassignedBooking.FlightItinerary.Add(new KeyValuePair<string, DateTime>(flightNumber, foundFlight.Value));

                Trace.WriteLine($"Iteration {pocet++}");
            }

            // Uložte změny zpět do databáze
            dbContext.SaveChanges();
        }

        public KeyValuePair<int, DateTime> FindFlightInformation(
           Dictionary<int, DateTime> notLoadedFlights,
           int numberOfLinkedPassengers,
           Dictionary<int, int> flightCounts)
        {
            var availableFlight = notLoadedFlights.FirstOrDefault(flight =>
                flightCounts[flight.Key] + numberOfLinkedPassengers <= 189);

            flightCounts[availableFlight.Key] += numberOfLinkedPassengers;

            return availableFlight;
        }
    }
}
