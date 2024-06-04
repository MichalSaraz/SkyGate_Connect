using Core.FlightContext;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class FlightRepository : BaseFlightRepository, IFlightRepository
    {
        private readonly IMemoryCache _cache;

        public FlightRepository(AppDbContext context, IMemoryCache cache) : base(context)
        {
            _cache = cache;
        }

        public async Task<Flight> GetFlightByCriteriaAsync(
        Expression<Func<Flight, bool>> criteria, bool tracked = false)
        {
            var flightQuery = _context.Set<Flight>().AsQueryable()
                .Include(_ => _.Airline)
                .Include(_ => _.ListOfBookedPassengers)
                .Where(criteria);

            if (!tracked)
            {
                flightQuery = flightQuery.AsNoTracking();
            }

            var flight = await flightQuery.FirstOrDefaultAsync();

            return flight;
        }

        public async Task<IReadOnlyList<Flight>> GetFlightsByCriteriaAsync(
            Expression<Func<Flight, bool>> criteria, bool tracked = false)
        {
            var flightsQuery = _context.Set<Flight>().AsQueryable()
                .Include(_ => _.ListOfBookedPassengers)
                .Include(_ => _.Seats)
                .Where(criteria);

            if (!tracked)
            {
                flightsQuery = flightsQuery.AsNoTracking();
            }

            var flights = await flightsQuery.ToListAsync();

            return flights;
        }

        public override async Task<BaseFlight> GetFlightByIdAsync(Guid id, bool tracked = true)
        {
            var CACHE_KEY = $"Flight_{id}";

            if (!tracked && _cache.TryGetValue(CACHE_KEY, out BaseFlight cachedFlight))
            {
                return cachedFlight;
            }

            var flight = await base.GetFlightByIdAsync(id, tracked) as Flight;

            _cache.Set(CACHE_KEY, flight, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });

            return flight;
        }
    }
}
