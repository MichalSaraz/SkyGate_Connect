using Core.Interfaces;
using Core.PassengerContext.APIS;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public interface IAPISDataRepository : IGenericRepository<APISData>
    {
        Task<APISData> GetAPISDataByCriteriaAsync(Expression<Func<APISData, bool>> criteria, bool tracked = true);
    }
}