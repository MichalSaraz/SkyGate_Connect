using Core.FlightContext;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public FlightRepository(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Flight> GetFlightByCriteriaAsync(
            Expression<Func<Flight, bool>> criteria, bool tracked = false)
        {
            var CACHE_KEY = $"Flight_{criteria}";

            var flightCache = _cache.Get<Flight>(CACHE_KEY);
            if (!tracked && flightCache != null)
            {
                return flightCache;
            }

            var flightQuery = _context.Flights.AsQueryable()
            .Include(_ => _.ScheduledFlight)
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
            var flightsQuery = _context.Flights.AsQueryable()
                .Include(_ => _.ScheduledFlight)
                .Include(_ => _.ListOfBookedPassengers)
                .Where(criteria);

            if (!tracked)
            {
                flightsQuery = flightsQuery.AsNoTracking();
            }

            var flights = await flightsQuery.ToListAsync();

            return flights;
        }

        public async Task<Flight> GetFlightByIdAsync(int id, bool tracked = true)
        {
            var CACHE_KEY = $"Flight_{id}";

            var flightCache = _cache.Get<Flight>(CACHE_KEY);
            if (!tracked && flightCache != null)
            {
                return flightCache;
            }

            var flightQuery = _context.Flights.AsQueryable()
            .Include(_ => _.ScheduledFlight)
                .ThenInclude(_ => _.Airline)
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
