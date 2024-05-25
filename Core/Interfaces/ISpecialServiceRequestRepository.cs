using Core.PassengerContext.JoinClasses;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface ISpecialServiceRequestRepository : IGenericRepository<SpecialServiceRequest>
    {
        Task<IReadOnlyList<SpecialServiceRequest>> GetSpecialServiceRequestsByCriteriaAsync(Expression<Func<SpecialServiceRequest, bool>> criteria);
    }
}