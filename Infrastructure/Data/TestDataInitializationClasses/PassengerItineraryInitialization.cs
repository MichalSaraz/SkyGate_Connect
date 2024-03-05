using Core.FlightContext;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.JoinClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.TestDataInitializationClasses
{
    public class PassengerItineraryInitialization
    {
        private readonly AppDbContext dbContext;
        private readonly Random random = new Random();

        public PassengerItineraryInitialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializePassengerItinerary()
        {
            var flights = dbContext.Flights.OfType<Flight>().ToList();
            var paxInfo = dbContext.PassengerInfo.ToList();
            var bookingReferences = dbContext.BookingReferences.ToList();
            List<KeyValuePair<string, DateTime>> notLoadedFlights = new List<KeyValuePair<string, DateTime>>();

            var passengerCountPerFlight = GeneratePassengersForFlights(flights);
            var totalBookedPassengers = GenerateKeysForFlights(flights);

            for (int i = 0; i < flights.Count; i++)
            {
                var flight = flights[i].ScheduledFlightId;
                var dateForNotLoadedFlights = flights[i].DepartureDateTime.AddDays(8);
                notLoadedFlights.Add(new KeyValuePair<string, DateTime>(flight, dateForNotLoadedFlights));
            };

            int pocet = 0;

            foreach (var bookingReference in bookingReferences)
            {
                // Počet letů pro každý PNR bude buď 1 nebo 2 s 50% pravděpodobností
                int numberOfFlights = random.Next(1, 3);

                // Přiřaďte existující cestující k itinerářům
                bookingReference.LinkedPassengers = dbContext.PassengerInfo
                    .Where(p => p.PNRId == bookingReference.PNR)
                    .ToList();

                // Vyberte náhodné lety podle pravidel
                var selectedFlights = SelectFlightsWithRules(
                    flights,
                    numberOfFlights,
                    bookingReference.LinkedPassengers.Count,
                    passengerCountPerFlight,
                    totalBookedPassengers);

                foreach (var flight in selectedFlights)
                {
                    // Přidejte cestující k letu
                    foreach (var passengerInfo in bookingReference.LinkedPassengers.ToList())
                    {
                        Passenger passenger = new Passenger();
                        passenger.MapFromPassengerInfo(passengerInfo);
                    }

                    totalBookedPassengers[flight.Id] += bookingReference.LinkedPassengers.Count;
                    Trace.WriteLine($"Flight {flight.Id}");
                    Trace.WriteLine($"BookedPax {totalBookedPassengers[flight.Id]}");
                }

                // Vygenerujte a přiřaďte itineráře pro daný PNR a lety
                bookingReference.FlightItinerary = GenerateAndAssignItinerary(selectedFlights);

                Trace.WriteLine($"Iteration {pocet++}");
            }

            var PNRWithoutItinerary = dbContext.BookingReferences.Where(b => b.FlightItinerary.Count == 0).ToList();

            foreach (var unassignedBooking in PNRWithoutItinerary)
            {
                var foundFlight = FindFlightInformation(notLoadedFlights, PNRWithoutItinerary);
                unassignedBooking.FlightItinerary.Add(new KeyValuePair<string, DateTime>(foundFlight.Key, foundFlight.Value));

                Trace.WriteLine($"Iteration {pocet++}");
            }

            // Uložte změny zpět do databáze
            dbContext.SaveChanges();
        }

        public KeyValuePair<string, DateTime> FindFlightInformation(
            List<KeyValuePair<string, DateTime>> notLoadedFlights,
            List<BookingReference> PNRWithoutItinerary)
        {
            return notLoadedFlights.FirstOrDefault(flight =>
                PNRWithoutItinerary.Sum(bookingReference =>
                    bookingReference.LinkedPassengers.Count(passenger =>
                        bookingReference.FlightItinerary.Any(itinerary =>
                            itinerary.Key == flight.Key))) <= 189);
        }

        private List<Flight> SelectFlightsWithRules(
            List<Flight> flights,
            int numberOfFlights,
            int linkedPassengers,
            Dictionary<int, int> passengerCountPerFlight,
            Dictionary<int, int> totalBookedPassengers)
        {
            var scheduledFlights = dbContext.ScheduledFlights.ToList();
            List<Flight> selectedFlights = new List<Flight>();

            DateTime departureDateTime = DateTime.UtcNow;
            var arrivalAirport = "";

            for (int i = 0; i < numberOfFlights; i++)
            {
                // Vyber lety, které odpovídají pravidlům
                var validFlights = GetValidFlights(
                    flights,
                    departureDateTime,
                    linkedPassengers,
                    arrivalAirport,
                    passengerCountPerFlight,
                    totalBookedPassengers);

                // Pokud nejsou dostupné žádné vhodné lety, přerušte generování
                if (validFlights.Count < numberOfFlights)
                {
                    break;
                }

                var random = new Random();

                // ...
                var selectedFlightIndex = random.Next(0, validFlights.Count);
                var selectedFlight = validFlights[selectedFlightIndex];

                // Přidání vybraného letu do seznamu
                selectedFlights.Add(selectedFlight);

                // Nastavení nového času pro další let
                departureDateTime = selectedFlight.ArrivalDateTime ?? DateTime.MinValue;
                arrivalAirport = scheduledFlights.SingleOrDefault(f => f.FlightNumber == selectedFlight.ScheduledFlightId).DestinationTo;
            }

            return selectedFlights;
        }

        private List<Flight> GetValidFlights(
            List<Flight> flights,
            DateTime departureDateTime,
            int linkedPassengers,
            string arrivalAirport,
            Dictionary<int, int> passengerCountPerFlight,
            Dictionary<int, int> totalBookedPassengers)
        {
            if (arrivalAirport == "")
            {
                return flights
                    .Where(f => totalBookedPassengers[f.Id] + linkedPassengers <= passengerCountPerFlight.SingleOrDefault(p => p.Key == f.Id).Value)
                    .OrderBy(f => f.DepartureDateTime)
                    .ToList();
            }
            else
            {
                return flights
                    .Where(f => f.DepartureDateTime > departureDateTime.AddHours(1) && f.DepartureDateTime < departureDateTime.AddHours(12))
                    .Where(f => dbContext.ScheduledFlights.Where(s => s.DestinationFrom == arrivalAirport).Select(s => s.FlightNumber).Contains(f.ScheduledFlightId))
                    .Where(f => totalBookedPassengers[f.Id] + linkedPassengers <= passengerCountPerFlight.SingleOrDefault(p => p.Key == f.Id).Value)
                    .OrderBy(f => f.DepartureDateTime)
                    .ToList();
            }
        }

        private List<KeyValuePair<string, DateTime>> GenerateAndAssignItinerary(List<Flight> selectedFlights)
        {
            List<KeyValuePair<string, DateTime>> itinerary = new List<KeyValuePair<string, DateTime>>();

            foreach (var flight in selectedFlights)
            {
                itinerary.Add(new KeyValuePair<string, DateTime>(flight.ScheduledFlightId, flight.DepartureDateTime));
            }

            return itinerary;
        }

        private int GeneratePassengersPerFlight()
        {
            int randomNumber = random.Next(1, 101);
            if (randomNumber <= 2)
                return random.Next(10, 31);
            else if (randomNumber <= 10)
                return random.Next(31, 81);
            else if (randomNumber <= 40)
                return random.Next(81, 121);
            else if (randomNumber <= 98)
                return random.Next(121, 187);
            else
                return random.Next(187, 193);
        }

        private Dictionary<int, int> GeneratePassengersForFlights(List<Flight> flights)
        {
            Dictionary<int, int> passengersPerFlight = new Dictionary<int, int>();

            foreach (var flight in flights)
            {
                int passengersCount = GeneratePassengersPerFlight();
                passengersPerFlight.Add(flight.Id, passengersCount);
            }

            return passengersPerFlight;
        }

        private Dictionary<int, int> GenerateKeysForFlights(List<Flight> flights)
        {
            Dictionary<int, int> keys = new Dictionary<int, int>();

            foreach (var flight in flights)
            {
                keys.Add(flight.Id, 0);
            }

            return keys;
        }
    }
}