using Core.BaggageContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IBaggageRepository : IGenericRepository<Baggage>
    {
        Task<Baggage> GetBaggageByTagNumber(string tagNumber);
        Task<IReadOnlyList<Baggage>> GetAllBaggageByCriteriaAsync(Expression<Func<Baggage, bool>> criteria);
        Task<Baggage> GetBaggageByIdAsync(Guid id, bool tracked = true);
        int GetNextSequenceValue(string sequenceName);
    }
}
