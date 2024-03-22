using Core.PassengerContext.APIS;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class APISDataRepository : GenericRepository<APISData>, IAPISDataRepository
    {
        public APISDataRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<APISData> GetAPISDataByCriteriaAsync(Expression<Func<APISData, bool>> criteria, bool tracked = true)
        {
            var apisDataQuery = _context.APISData.AsQueryable()
                .Where(criteria);

            if (!tracked)
            {
                apisDataQuery = apisDataQuery.AsNoTracking();
            }

            var apisData = await apisDataQuery.FirstOrDefaultAsync();

            return apisData;
        }
    }
}
