using System.Diagnostics;
using Infrastructure.Data;

namespace TestProject.TestDataInitializationClasses
{
    public class ReservedSeatsInitialization
    {
        private readonly AppDbContext dbContext;
        private readonly Random random = new();

        public ReservedSeatsInitialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializeSeatReservations()
        {
            var seatList = _GenerateSeatList();
            var bookingReferences = dbContext.BookingReferences.ToList();
            var passengerInfos = dbContext.PassengerInfo.ToList();
            var seatsOccupied = new Dictionary<string, List<string>>();

            foreach (var bookingReference in bookingReferences.Where((_, i) => (i + 1) % 5 == 0))
            {
                foreach (var flight in bookingReference.FlightItinerary)
                {
                    var flightIdentifier = $"{flight.Key}{flight.Value}";

                    if (!seatsOccupied.TryGetValue(flightIdentifier, out var value))
                    {
                        value = new List<string>();
                        seatsOccupied[flightIdentifier] = value;
                    }

                    var seatsForFlight = value;

                    var passengers = bookingReference.LinkedPassengers.ToList();

                    var seatsToAssign = seatList.Where(seat => !seatsForFlight.Contains(seat))
                        .Skip(random.Next(seatList.Count - passengers.Count))
                        .Take(passengers.Count)
                        .ToList();

                    if (seatsToAssign.Count >= passengers.Count)
                    {
                        var i = 0;
                        seatsForFlight.AddRange(seatsToAssign);

                        foreach (var passenger in passengers)
                        {
                            var passengerInfo = passengerInfos.SingleOrDefault(p => p.Id == passenger.Id);

                            if (passengerInfo != null)
                            {
                                passengerInfo.ReservedSeats ??= new Dictionary<string, string>();

                                passengerInfo.ReservedSeats.Add(flight.Key, seatsToAssign[i]);

                                Trace.WriteLine($"Seat Assigned {seatsToAssign[i]}");
                            }

                            i++;
                        }
                    }
                }
            }

            dbContext.SaveChanges();
        }

        private static List<string> _GenerateSeatList()
        {
            var seatList = new List<string>();

            for (int i = 2; i <= 30; i++)
            {
                for (char c = 'A'; c <= 'F'; c++)
                {
                    seatList.Add($"{i}{c}");
                }
            }

            return seatList;
        }
    }
}