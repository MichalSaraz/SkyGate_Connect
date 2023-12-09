using Core.PassengerContext.Booking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Infrastructure.Data
{
    public class ReservedSeatsInitialization
    {
        private readonly AppDbContext dbContext;
        private readonly Random random = new Random();

        public ReservedSeatsInitialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializeSeatReservations()
        {
            var seatList = GenerateSeatList();
            var bookingReferences = dbContext.BookingReferences.ToList();
            var passengerInfos = dbContext.PassengerInfos.ToList();
            var seatsOccupied = new Dictionary<string, List<string>>();

            foreach (var bookingReference in bookingReferences.Where((x, i) => (i + 1) % 5 == 0))
            {
                foreach (var flight in bookingReference.FlightItinerary)
                {
                    var flightIdentifier = $"{flight.Key}{flight.Value}";

                    if (!seatsOccupied.ContainsKey(flightIdentifier))
                    {
                        seatsOccupied[flightIdentifier] = new List<string>();
                    }

                    var seatsForFlight = seatsOccupied[flightIdentifier];

                    var passengers = bookingReference.LinkedPassengers.ToList();
                    
                    var seatsToAssign = seatList
                        .Where(seat => !seatsForFlight.Contains(seat))
                        .Skip(random.Next(seatList.Count - passengers.Count))
                        .Take(passengers.Count)
                        .ToList();
                    
                    if (seatsToAssign.Count >= passengers.Count)
                    {
                        int i = 0;
                        seatsForFlight.AddRange(seatsToAssign);

                        foreach (var passenger in passengers)
                        {
                            var passengerInfo = passengerInfos.SingleOrDefault(p => p.Id == passenger.Id);

                            if (passengerInfo != null)
                            {
                                if (passengerInfo.ReservedSeats == null)
                                {
                                    passengerInfo.ReservedSeats = new Dictionary<string, string>();
                                }

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

        private static List<string> GenerateSeatList()
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
