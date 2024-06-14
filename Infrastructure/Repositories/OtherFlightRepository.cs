using Core.FlightContext;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class OtherFlightRepository : BaseFlightRepository, IOtherFlightRepository
    {
        public OtherFlightRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<OtherFlight> GetOtherFlightByCriteriaAsync(Expression<Func<OtherFlight, bool>> criteria,
            bool tracked = false)
        {
            var flightQuery = _context.Set<OtherFlight>()
                .AsQueryable()
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
    }
}
