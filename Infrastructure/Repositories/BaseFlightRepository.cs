using Core.FlightContext;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BaseFlightRepository<T> : GenericRepository<T>, IBaseFlightRepository<T> where T : BaseFlight
    {
        protected BaseFlightRepository(AppDbContext context) : base(context)
        {
        }

        public virtual async Task<T> GetFlightByCriteriaAsync(
        Expression<Func<T, bool>> criteria, bool tracked = false)
        {
            var flightQuery = _context.Set<T>().AsQueryable()
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
