using Core.FlightContext.FlightInfo;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace Infrastructure.Repositories
{
    public class DestinationRepository : IDestinationRepository
    {
        private readonly AppDbContext _context;

        public DestinationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Destination> GetDestinationByCriteriaAsync(Expression<Func<Destination, bool>> criteria)
        {
            return await _context.Destinations.AsNoTracking()
                .Where(criteria)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Destination>> GetDestinationsByCriteriaAsync(Expression<Func<Destination, bool>> criteria)
        {
            return await _context.Destinations.AsNoTracking()
                .Include(_ => _.Country)
                .Include(_ => _.Departures)
                .Include(_ => _.Arrivals)
                .Where(criteria)
                .ToListAsync();
        }
    }
}
