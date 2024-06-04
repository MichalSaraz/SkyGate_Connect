using Core.PassengerContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IBasePassengerOrItemRepository : IGenericRepository<BasePassengerOrItem>
    {
        Task<IReadOnlyList<BasePassengerOrItem>> GetBasePassengerOrItemsByCriteriaAsync(
            Expression<Func<BasePassengerOrItem, bool>> criteria, bool tracked = true);
    }
}