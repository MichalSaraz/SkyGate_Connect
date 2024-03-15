using Core.FlightContext;
using Core.FlightContext.FlightInfo;
using Infrastructure.Data;

namespace TestProject.TestDataInitializationClasses
{
    public class ScheduledFlightsCleanup
    {
        private readonly AppDbContext dbContext;

        public ScheduledFlightsCleanup(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void RemoveDuplicates()
        {
            var duplicateGroups = dbContext.Set<ScheduledFlight>()
                .GroupBy(sf => sf.FlightNumber.Substring(2))
                .ToList()
                .Where(group => group.Count() > 1);

            var duplicatesToRemove = duplicateGroups.SelectMany(group =>
                    group.Where(sf => _ShouldRemoveScheduledFlight(sf, dbContext.Set<Destination>())))
                .ToList();

            foreach (var duplicate in duplicatesToRemove)
            {
                var relatedFlights = dbContext.Set<Flight>()
                    .Where(f => f.ScheduledFlightId == duplicate.FlightNumber)
                    .ToList();

                dbContext.Set<Flight>().RemoveRange(relatedFlights);
                dbContext.Set<ScheduledFlight>().Remove(duplicate);
            }

            dbContext.SaveChanges();
        }

        private static bool _ShouldRemoveScheduledFlight(ScheduledFlight scheduledFlight,
            IQueryable<Destination> destinations)
        {
            var destinationIds = new[] { scheduledFlight.DestinationFrom, scheduledFlight.DestinationTo };

            foreach (var destinationId in destinationIds)
            {
                var destination = destinations.SingleOrDefault(d => d.IATAAirportCode == destinationId);

                if (destination == null) continue;

                switch (destination.CountryId)
                {
                    case "NO" when scheduledFlight.FlightNumber.StartsWith("D8"):
                    case "SE":
                    case "DK" when scheduledFlight.FlightNumber.StartsWith("DY"):
                        return true;
                }
            }

            return false;
        }
    }
}