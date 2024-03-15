using Core.FlightContext;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class FlightRepository<T> : BaseFlightRepository<T>, IFlightRepository<T> where T : BaseFlight
    {
        private readonly IMemoryCache _cache;

        public FlightRepository(AppDbContext context, IMemoryCache cache) : base(context)
        {
            _cache = cache;
        }

        public async Task<TFlight> GetFlightByCriteriaAsync<TFlight>(
            Expression<Func<TFlight, bool>> criteria, bool tracked = false) where TFlight : BaseFlight
        {
            var CACHE_KEY = $"Flight_{criteria}";

            var flightCache = _cache.Get<TFlight>(CACHE_KEY);
            if (!tracked && flightCache != null)
            {
                return flightCache;
            }

            var flightQuery = _context.Set<TFlight>().AsQueryable()
                .Include(_ => _.ListOfBookedPassengers)
                .Where(criteria);

            if (!tracked)
            {
                flightQuery = flightQuery.AsNoTracking();
            }

            var flight = await flightQuery.FirstOrDefaultAsync();

            _cache.Set(CACHE_KEY, flight, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });

            return flight;
        }

        public async Task<IReadOnlyList<Flight>> GetFlightsByCriteriaAsync(
            Expression<Func<Flight, bool>> criteria, bool tracked = false)
        {
            var flightsQuery = _context.Flights.OfType<Flight>().AsQueryable()
                .Include(_ => _.ListOfBookedPassengers)
                .Where(criteria);

            if (!tracked)
            {
                flightsQuery = flightsQuery.AsNoTracking();
            }

            var flights = await flightsQuery.ToListAsync();

            return flights;
        }

        public async Task<TFlight> GetFlightByIdAsync<TFlight>(int id, bool tracked = true) where TFlight : BaseFlight
        {
            var CACHE_KEY = $"Flight_{id}";

            var flightCache = _cache.Get<TFlight>(CACHE_KEY);
            if (!tracked && flightCache != null)
            {
                return flightCache;
            }

            var flightQuery = _context.Set<TFlight>().AsQueryable()
                .Include(_ => _.Airline)
                .Include(_ => _.ListOfBookedPassengers)
                .Where(_ => _.Id == id);

            if (!tracked)
            {
                flightQuery = flightQuery.AsNoTracking();
            }

            var flight = await flightQuery.SingleOrDefaultAsync();

            _cache.Set(CACHE_KEY, flight, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });

            return flight;
        }
    }
}
