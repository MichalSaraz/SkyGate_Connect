using Core.FlightContext;
using Core.Interfaces;
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
    public class BaseFlightRepository<T> : GenericRepository<T>, IBaseFlightRepository<T> where T : BaseFlight
    {
        public BaseFlightRepository(AppDbContext context) : base(context)
        {
        }

        public virtual async Task<T> GetFlightByCriteriaAsync(
        Expression<Func<T, bool>> criteria, bool tracked = false)
        {
            var flightQuery = _context.Set<T>().AsQueryable()
                .Include(f => f.ListOfBookedPassengers)
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
