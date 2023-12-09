using Core.FlightContext;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class FlightInitializationService
    {
        private readonly AppDbContext dbContext;
        private readonly ITimeProvider timeProvider;

        public FlightInitializationService(AppDbContext dbContext, ITimeProvider timeProvider)
        {
            this.dbContext = dbContext;
            this.timeProvider = timeProvider;
        }



        public void CreateFlightsForNext7Days()
        {
            DateTime currentDate = timeProvider.Now.Date;

            var scheduledFlights = dbContext.ScheduledFlights.ToList();
            var flights = dbContext.Flights.ToList();

            int maxId = flights.Count > 0 ? flights.Max(f => f.Id) : 0;

            for (int i = 0; i < 7; i++)
            {
                DateTime targetDate = currentDate.AddDays(i);
                int dayOfWeek = (int)targetDate.DayOfWeek;

                foreach (var scheduledFlight in scheduledFlights)
                {
                    var matchingDepartureTime = scheduledFlight.DepartureTimes.FirstOrDefault(dt => (int)dt.Key == dayOfWeek);

                    if (!matchingDepartureTime.Equals(default(KeyValuePair<DayOfWeek, TimeSpan>)) && scheduledFlight.DepartureTimes.Any(dt => (int)dt.Key == dayOfWeek))
                    {
                        maxId++; // Inkrementace Id

                        var arrivalTime = scheduledFlight.ArrivalTimes.First(at => (int)at.Key == dayOfWeek);

                        var flight = new Flight
                        {
                            Id = maxId,
                            ScheduledFlightId = scheduledFlight.FlightNumber,
                            DepartureDateTime = targetDate.Add(matchingDepartureTime.Value),
                            ArrivalDateTime = targetDate.Add(arrivalTime.Value)
                        };

                        dbContext.Flights.Add(flight);
                    }
                }
            }

            dbContext.SaveChanges();

            //ToDo - Solve problem with AddDays(1), when ArrivalDateTime is after midnight
        }
    }
}
