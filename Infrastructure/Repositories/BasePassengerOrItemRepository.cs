using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.JoinClasses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BasePassengerOrItemRepository : GenericRepository<BasePassengerOrItem>, IBasePassengerOrItemRepository
    {
        public BasePassengerOrItemRepository(AppDbContext context) : base(context)
        {            
        }

        public async Task<IReadOnlyList<BasePassengerOrItem>> GetBasePassengerOrItemByCriteriaAsync(
            Expression<Func<BasePassengerOrItem, bool>> criteria)
        {
            return await _context.Passengers.AsQueryable().AsNoTracking()
                .Where(criteria)
                .ToListAsync();
        }
    }
}
