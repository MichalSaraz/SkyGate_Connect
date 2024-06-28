using Core.FlightContext;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BaseFlightRepository : GenericRepository<BaseFlight>, IBaseFlightRepository
    {
        public BaseFlightRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<BaseFlight>> GetFlightsByCriteriaAsync(
            Expression<Func<BaseFlight, bool>> criteria, bool tracked = false)
        {
            var flightsQuery = _context.Flights.AsQueryable()
                .Include(_ => _.ListOfBookedPassengers)
                .Where(criteria);

            if (!tracked)
            {
                flightsQuery = flightsQuery.AsNoTracking();
            }

            var flights = await flightsQuery.ToListAsync();

            return flights;
        }

        public virtual async Task<BaseFlight> GetFlightByIdAsync(Guid id, bool tracked = true, bool displayDetails = false)
        {
            var flightQuery = _context.Flights.AsQueryable()
                .Include(_ => _.Airline)
                .Include(_ => _.ListOfBookedPassengers)
                .Where(_ => _.Id == id);            

            if (!tracked)
            {
                flightQuery = flightQuery.AsNoTracking();
            }

            if (displayDetails && await flightQuery.OfType<Flight>().AnyAsync())
            {
                flightQuery = flightQuery.OfType<Flight>()
                    .Include(_ => _.Seats)
                    .Include(_ => _.ScheduledFlight)
                    .Include(_ => _.Aircraft)
                    .Include(_ => _.DestinationTo)
                    .Include(_ => _.ListOfBookedPassengers)
                        .ThenInclude(_ => _.PassengerOrItem);
            }

            var flight = await flightQuery.FirstOrDefaultAsync();

            return flight;
        }
    }
}
