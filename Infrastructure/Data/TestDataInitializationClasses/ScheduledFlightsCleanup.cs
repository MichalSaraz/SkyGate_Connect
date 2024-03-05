using Core.FlightContext.FlightInfo;
using Core.FlightContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.TestDataInitializationClasses
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

            var duplicatesToRemove = duplicateGroups
                .SelectMany(group => group.Where(sf => ShouldRemoveScheduledFlight(sf, dbContext.Set<Destination>())))
                .ToList();

            foreach (var duplicate in duplicatesToRemove)
            {
                var relatedFlights = dbContext.Set<Flight>().Where(f => f.ScheduledFlightId == duplicate.FlightNumber).ToList();
                dbContext.Set<Flight>().RemoveRange(relatedFlights);
                dbContext.Set<ScheduledFlight>().Remove(duplicate);
            }

            dbContext.SaveChanges();
        }

        private bool ShouldRemoveScheduledFlight(ScheduledFlight scheduledFlight, IQueryable<Destination> destinations)
        {
            var destinationIds = new[] { scheduledFlight.DestinationFrom, scheduledFlight.DestinationTo };

            foreach (var destinationId in destinationIds)
            {
                var destination = destinations.SingleOrDefault(d => d.IATAAirportCode == destinationId);

                if (destination != null)
                {
                    if (destination.CountryId == "NO" && scheduledFlight.FlightNumber.StartsWith("D8"))
                    {
                        return true;
                    }

                    if (destination.CountryId == "SE" || destination.CountryId == "DK" && scheduledFlight.FlightNumber.StartsWith("DY"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
