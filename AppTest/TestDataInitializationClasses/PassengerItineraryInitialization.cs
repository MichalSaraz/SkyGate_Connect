using System.Diagnostics;
using Core.FlightContext;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Infrastructure.Data;

namespace TestProject.TestDataInitializationClasses
{
    public class PassengerItineraryInitialization
    {
        private readonly AppDbContext dbContext;
        private readonly Random random = new();

        public PassengerItineraryInitialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializePassengerItinerary()
        {
            var flights = dbContext.Flights.OfType<Flight>().ToList();
            var bookingReferences = dbContext.BookingReferences.ToList();
            var notLoadedFlights = new List<KeyValuePair<string, DateTime>>();

            var passengerCountPerFlight = _GeneratePassengersForFlights(flights);
            var totalBookedPassengers = _GenerateKeysForFlights(flights);

            foreach (var t in flights)
            {
                var flight = t.ScheduledFlightId;
                var dateForNotLoadedFlights = t.DepartureDateTime.AddDays(8);
                notLoadedFlights.Add(new KeyValuePair<string, DateTime>(flight, dateForNotLoadedFlights));
            }

            var count = 0;

            foreach (var bookingReference in bookingReferences)
            {
                var numberOfFlights = random.Next(1, 3);

                bookingReference.LinkedPassengers = dbContext.PassengerInfo
                    .Where(p => p.PNRId == bookingReference.PNR)
                    .ToList();

                var selectedFlights = _SelectFlightsWithRules(flights, numberOfFlights,
                    bookingReference.LinkedPassengers.Count, passengerCountPerFlight, totalBookedPassengers);

                foreach (var flight in selectedFlights)
                {
                    foreach (var passengerInfo in bookingReference.LinkedPassengers.ToList())
                    {
                        var passenger = new Passenger();
                        passenger.MapFromPassengerInfo(passengerInfo);
                    }

                    totalBookedPassengers[flight.Id] += bookingReference.LinkedPassengers.Count;
                    Trace.WriteLine($"Flight {flight.Id}");
                    Trace.WriteLine($"BookedPax {totalBookedPassengers[flight.Id]}");
                }

                bookingReference.FlightItinerary = _GenerateAndAssignItinerary(selectedFlights);

                Trace.WriteLine($"Iteration {count++}");
            }

            var PNRWithoutItinerary = dbContext.BookingReferences.Where(b => b.FlightItinerary.Count == 0).ToList();

            foreach (var unassignedBooking in PNRWithoutItinerary)
            {
                var foundFlight = _FindFlightInformation(notLoadedFlights, PNRWithoutItinerary);

                unassignedBooking.FlightItinerary.Add(
                    new KeyValuePair<string, DateTime>(foundFlight.Key, foundFlight.Value));

                Trace.WriteLine($"Iteration {count++}");
            }

            dbContext.SaveChanges();
        }

        private static KeyValuePair<string, DateTime> _FindFlightInformation(
            IEnumerable<KeyValuePair<string, DateTime>> notLoadedFlights,
            IReadOnlyCollection<BookingReference> PNRWithoutItinerary)
        {
            return notLoadedFlights.FirstOrDefault(flight => PNRWithoutItinerary.Sum(bookingReference =>
                bookingReference.LinkedPassengers.Count(_ =>
                    bookingReference.FlightItinerary.Any(itinerary => itinerary.Key == flight.Key))) <= 189);
        }

        private List<Flight> _SelectFlightsWithRules(IReadOnlyCollection<Flight> flights, int numberOfFlights,
            int linkedPassengers, Dictionary<int, int> passengerCountPerFlight,
            IReadOnlyDictionary<int, int> totalBookedPassengers)
        {
            var scheduledFlights = dbContext.ScheduledFlights.ToList();
            var selectedFlights = new List<Flight>();

            var departureDateTime = DateTime.UtcNow;
            var arrivalAirport = "";

            for (int i = 0; i < numberOfFlights; i++)
            {
                var validFlights = _GetValidFlights(flights, departureDateTime, linkedPassengers, arrivalAirport,
                    passengerCountPerFlight, totalBookedPassengers);

                if (validFlights.Count < numberOfFlights)
                {
                    break;
                }

                var selectedFlightIndex = random.Next(0, validFlights.Count);
                var selectedFlight = validFlights[selectedFlightIndex];

                selectedFlights.Add(selectedFlight);

                departureDateTime = selectedFlight.ArrivalDateTime ?? DateTime.MinValue;
                arrivalAirport = scheduledFlights
                    .SingleOrDefault(f => f.FlightNumber == selectedFlight.ScheduledFlightId)
                    ?.DestinationTo;
            }

            return selectedFlights;
        }

        private List<Flight> _GetValidFlights(IEnumerable<Flight> flights, DateTime departureDateTime,
            int linkedPassengers, string? arrivalAirport, Dictionary<int, int> passengerCountPerFlight,
            IReadOnlyDictionary<int, int> totalBookedPassengers)
        {
            if (arrivalAirport == "")
            {
                return flights
                    .Where(f => totalBookedPassengers[f.Id] + linkedPassengers <= passengerCountPerFlight
                        .SingleOrDefault(p => p.Key == f.Id)
                        .Value)
                    .OrderBy(f => f.DepartureDateTime)
                    .ToList();
            }

            return flights
                .Where(f => f.DepartureDateTime > departureDateTime.AddHours(1) &&
                            f.DepartureDateTime < departureDateTime.AddHours(12))
                .Where(f => dbContext.ScheduledFlights.Where(s => s.DestinationFrom == arrivalAirport)
                    .Select(s => s.FlightNumber)
                    .Contains(f.ScheduledFlightId))
                .Where(f => totalBookedPassengers[f.Id] + linkedPassengers <= passengerCountPerFlight
                    .SingleOrDefault(p => p.Key == f.Id)
                    .Value)
                .OrderBy(f => f.DepartureDateTime)
                .ToList();
        }

        private static List<KeyValuePair<string, DateTime>> _GenerateAndAssignItinerary(
            IEnumerable<Flight> selectedFlights)
        {
            return selectedFlights.Select(flight =>
                    new KeyValuePair<string, DateTime>(flight.ScheduledFlightId, flight.DepartureDateTime))
                .ToList();
        }

        private int _GeneratePassengersPerFlight()
        {
            var randomNumber = random.Next(1, 101);

            return randomNumber switch
            {
                <= 2 => random.Next(10, 31),
                <= 10 => random.Next(31, 81),
                <= 40 => random.Next(81, 121),
                <= 98 => random.Next(121, 187),
                _ => random.Next(187, 193)
            };
        }

        private Dictionary<int, int> _GeneratePassengersForFlights(List<Flight> flights)
        {
            var passengersPerFlight = new Dictionary<int, int>();

            foreach (var flight in flights)
            {
                var passengersCount = _GeneratePassengersPerFlight();
                passengersPerFlight.Add(flight.Id, passengersCount);
            }

            return passengersPerFlight;
        }

        private static Dictionary<int, int> _GenerateKeysForFlights(IEnumerable<Flight> flights)
        {
            return flights.ToDictionary(flight => flight.Id, _ => 0);
        }
    }
}