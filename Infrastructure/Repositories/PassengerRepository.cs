using Core.FlightContext;
using Core.Interfaces;
using Core.PassengerContext;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class PassengerRepository : GenericRepository<Passenger>, IPassengerRepository
    {
        private readonly IMemoryCache _cache;

        public PassengerRepository(AppDbContext context, IMemoryCache cache) : base(context)
        {
            _cache = cache;
        }

        public async Task<IReadOnlyList<Passenger>> GetPassengersByCriteriaAsync(
            Expression<Func<Passenger, bool>> criteria, bool tracked = false)
        {
            var CACHE_KEY = $"Passengers_{criteria}";

            var passengersCache = _cache.Get<IReadOnlyList<Passenger>>(CACHE_KEY);
            if (!tracked && passengersCache != null)
            {
                return passengersCache;
            }

            var passengersQuery = _context.Passengers.AsQueryable()
                .Include(_ => _.PNR)
                .Include(_ => _.TravelDocuments)
                .Include(_ => _.Flights)
                    .ThenInclude(_ => _.Flight)
                        //.ThenInclude(_ => _.ScheduledFlight)
                .Include(_ => _.AssignedSeats)
                .Where(criteria);

            if (!tracked)
            {
                passengersQuery = passengersQuery.AsNoTracking();
            }

            var passengers = await passengersQuery.ToListAsync();

            _cache.Set(CACHE_KEY, passengers, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });

            return passengers;
        }

        public async Task<Passenger> GetPassengerByIdAsync(Guid id, bool tracked = true,
            bool displayDetails = false)
        {
            var CACHE_KEY = $"Passenger_{id}_{displayDetails}";

            var passengerCache = _cache.Get<Passenger>(CACHE_KEY);
            if (!tracked && passengerCache != null)
            {
                return passengerCache;
            }

            var passengerQuery = _context.Passengers.AsQueryable()
                .Include(_ => _.PNR)
                .Include(_ => _.Flights)
                    .ThenInclude(_ => _.Flight)                        
                .Where(_ => _.Id == id);

            if (displayDetails)
            {
                passengerQuery = passengerQuery
                .Include(_ => _.PassengerCheckedBags)
                    .ThenInclude(_ => _.BaggageTag)
                .Include(_ => _.PassengerCheckedBags)
                    .ThenInclude(_ => _.SpecialBag)
                .Include(_ => _.PassengerCheckedBags)
                    .ThenInclude(_ => _.FinalDestination)
                .Include(_ => _.PassengerCheckedBags)
                    .ThenInclude(_ => _.Flights)
                        .ThenInclude(_ => _.Flight)
                .Include(_ => _.AssignedSeats)
                .Include(_ => _.SpecialServiceRequests);
            }

            if (!tracked)
            {
                passengerQuery = passengerQuery.AsNoTracking();
            }

            var passenger = await passengerQuery.SingleOrDefaultAsync();

            _cache.Set(CACHE_KEY, passenger, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });

            return passenger;
        }

        public async Task<IReadOnlyList<Passenger>> GetPassengersWithFlightConnectionsAsync(int flightId, bool isOnwardFlight)
        {
            var passengerQuery = _context.Passengers.AsQueryable().AsNoTracking()
                .Include(_ => _.Flights)
                    .ThenInclude(_ => _.Flight)
                        //.ThenInclude(_ => _.ScheduledFlight)
                .Include(_ => _.PassengerCheckedBags)
                .Include(_ => _.AssignedSeats)
                .Where(p => p.Flights
                    .Any(_ => _.FlightId == flightId));

            if (isOnwardFlight)
            {
                passengerQuery = passengerQuery
                    .Where(p => p.Flights
                        .Any(_ => _.Flight.DepartureDateTime > p.Flights
                            .FirstOrDefault(_ => _.FlightId == flightId).Flight.DepartureDateTime));
            }
            else
            {
                passengerQuery = passengerQuery
                    .Where(p => p.Flights
                        .Any(_ => _.Flight.DepartureDateTime < p.Flights
                            .FirstOrDefault(_ => _.FlightId == flightId).Flight.DepartureDateTime));
            }

            return await passengerQuery.ToListAsync();
        }
    }
}
