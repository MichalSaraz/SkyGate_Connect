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

        public virtual async Task<BaseFlight> GetFlightByIdAsync(Guid id, bool tracked = true)
        {
            var flightQuery = _context.Flights.AsQueryable()
                .Include(_ => _.Airline)
                .Include(_ => _.ListOfBookedPassengers)
                .Where(_ => _.Id == id);

            if (!tracked)
            {
                flightQuery = flightQuery.AsNoTracking();
            }

            var flight = await flightQuery.FirstOrDefaultAsync();

            return flight;
        }
    }
}
