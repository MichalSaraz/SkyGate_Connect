using Core.PassengerContext.JoinClasses;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface ISpecialServiceRequestRepository : IGenericRepository<SpecialServiceRequest>
    {
        /// <summary>
        /// Retrieves special service requests based on the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria to filter the special service requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of special
        /// service requests.</returns>
        Task<IReadOnlyList<SpecialServiceRequest>> GetSpecialServiceRequestsByCriteriaAsync(
            Expression<Func<SpecialServiceRequest, bool>> criteria);
    }
}