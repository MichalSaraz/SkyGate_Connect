using System.Diagnostics;
using Core.FlightContext;
using Infrastructure.Data;

namespace TestProject.TestDataInitializationClasses
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
            var notLoadedFlights = new Dictionary<int, DateTime>();
            var flightCounts = new Dictionary<int, int>();

            foreach (var t in flights)
            {
                var flight = t.Id;
                var dateForNotLoadedFlights = t.DepartureDateTime.AddDays(8);
                notLoadedFlights.Add(flight, dateForNotLoadedFlights);
                flightCounts.Add(flight, 0);
            }

            var count = 0;

            var PNRWithoutItinerary = dbContext.BookingReferences.Where(b => b.FlightItinerary.Count == 0).ToList();

            foreach (var unassignedBooking in PNRWithoutItinerary)
            {
                unassignedBooking.LinkedPassengers = dbContext.PassengerBookingDetails
                    .Where(p => p.PNRId == unassignedBooking.PNR)
                    .ToList();

                var foundFlight = _FindFlightInformation(notLoadedFlights, unassignedBooking.LinkedPassengers.Count,
                    flightCounts);

                var flightNumber = flights.OfType<Flight>()
                    .SingleOrDefault(f => f.Id == foundFlight.Key)
                    ?.ScheduledFlightId;

                if (flightNumber != null)
                    unassignedBooking.FlightItinerary.Add(
                        new KeyValuePair<string, DateTime>(flightNumber, foundFlight.Value));

                Trace.WriteLine($"Iteration {count++}");
            }

            dbContext.SaveChanges();
        }

        private static KeyValuePair<int, DateTime> _FindFlightInformation(Dictionary<int, DateTime> notLoadedFlights,
            int numberOfLinkedPassengers, IDictionary<int, int> flightCounts)
        {
            var availableFlight =
                notLoadedFlights.FirstOrDefault(flight => flightCounts[flight.Key] + numberOfLinkedPassengers <= 189);

            flightCounts[availableFlight.Key] += numberOfLinkedPassengers;

            return availableFlight;
        }
    }
}