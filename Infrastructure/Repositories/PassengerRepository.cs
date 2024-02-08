using Core.Interfaces;
using Core.PassengerContext;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly AppDbContext _context;

        public PassengerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Passenger> GetPassengerByCriteriaAsync(Expression<Func<Passenger, bool>> criteria)
        {
            return await _context.Passengers
                .Include(_ => _.PNR)
                .Include(_ => _.FrequentFlyer)
                .Include(_ => _.TravelDocuments)
                .Include(_ => _.PassengerCheckedBags)
                    .ThenInclude(_ => _.BaggageTag)
                .Include(_ => _.PassengerCheckedBags)
                    .ThenInclude(_ => _.SpecialBag)
                .Include(_ => _.Flights)
                    .ThenInclude(_ => _.Flight)
                        .ThenInclude(_ => _.ScheduledFlight)
                            .ThenInclude(_ => _.Airline)
                .Include(_ => _.Comments)
                    .ThenInclude(_ => _.PredefinedComment)
                .Include(_ => _.AssignedSeats)
                .Include(_ => _.SpecialServiceRequests)
                    .ThenInclude(_ => _.SSRCode)
                .Where(criteria)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Passenger>> GetPassengersByCriteriaAsync(Expression<Func<Passenger, bool>> criteria)
        {
            return await _context.Passengers
                .Include(_ => _.PNR)
                .Include(_ => _.FrequentFlyer)
                .Include(_ => _.TravelDocuments)
                .Include(_ => _.PassengerCheckedBags)
                    .ThenInclude(_ => _.BaggageTag)
                .Include(_ => _.PassengerCheckedBags)
                    .ThenInclude(_ => _.SpecialBag)
                .Include(_ => _.Flights)
                    .ThenInclude(_ => _.Flight)
                        .ThenInclude(_ => _.ScheduledFlight)
                            .ThenInclude(_ => _.Airline)
                .Include(_ => _.Comments)
                    .ThenInclude(_ => _.PredefinedComment)
                .Include(_ => _.AssignedSeats)
                .Include(_ => _.SpecialServiceRequests)
                    .ThenInclude(_ => _.SSRCode)
                .Where(criteria)
                .ToListAsync();
        }
    }
}
