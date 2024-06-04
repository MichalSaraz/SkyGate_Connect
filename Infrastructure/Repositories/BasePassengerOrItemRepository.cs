using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.JoinClasses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BasePassengerOrItemRepository : GenericRepository<BasePassengerOrItem>, IBasePassengerOrItemRepository
    {
        public BasePassengerOrItemRepository(AppDbContext context) : base(context)
        {            
        }

        public async Task<IReadOnlyList<BasePassengerOrItem>> GetBasePassengerOrItemsByCriteriaAsync(
            Expression<Func<BasePassengerOrItem, bool>> criteria, bool tracked = true)
        {
            var basePassengerOrItemQuery = _context.Passengers.AsQueryable()
                .Include(_ => _.Flights)
                .Where(criteria);

            if (!tracked)
            {
                basePassengerOrItemQuery = basePassengerOrItemQuery.AsNoTracking();
            }

            var basePassengerOrItems = await basePassengerOrItemQuery.ToListAsync();
            return basePassengerOrItems;
        }
    }
}
