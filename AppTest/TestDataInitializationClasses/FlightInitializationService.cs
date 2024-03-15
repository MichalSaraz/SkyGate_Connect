using Core.FlightContext;
using Core.Interfaces;
using Infrastructure.Data;

namespace TestProject.TestDataInitializationClasses
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
            var currentDate = timeProvider.Now.Date;

            var scheduledFlights = dbContext.ScheduledFlights.ToList();
            var flights = dbContext.Flights.ToList();

            for (int i = 0; i < 7; i++)
            {
                var targetDate = currentDate.AddDays(i);
                var dayOfWeek = (int)targetDate.DayOfWeek;

                var flightsExistForDay = flights.Any(f => f.DepartureDateTime.Date == targetDate.Date);

                if (flightsExistForDay) continue;

                foreach (var scheduledFlight in scheduledFlights)
                {
                    var matchingDepartureTime =
                        scheduledFlight.DepartureTimes.FirstOrDefault(dt => (int)dt.Key == dayOfWeek);

                    if (!matchingDepartureTime.Equals(default(KeyValuePair<DayOfWeek, TimeSpan>)) &&
                        scheduledFlight.DepartureTimes.Any(dt => (int)dt.Key == dayOfWeek))
                    {
                        var arrivalTime = scheduledFlight.ArrivalTimes.First(at => (int)at.Key == dayOfWeek);

                        var flight = new Flight(
                            scheduledFlight.FlightNumber,
                            targetDate.Add(matchingDepartureTime.Value),
                            targetDate.Add(arrivalTime.Value),
                            scheduledFlight.DestinationFrom, 
                            scheduledFlight.DestinationTo, 
                            scheduledFlight.Airline
                        );

                        dbContext.Flights.Add(flight);
                    }
                }
            }

            dbContext.SaveChanges();
        }
    }
}