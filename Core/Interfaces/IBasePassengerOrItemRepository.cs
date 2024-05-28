using Core.PassengerContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IBasePassengerOrItemRepository : IGenericRepository<BasePassengerOrItem>
    {
        Task<IReadOnlyList<BasePassengerOrItem>> GetBasePassengerOrItemByCriteriaAsync(
            Expression<Func<BasePassengerOrItem, bool>> criteria, bool tracked = true);
    }
}