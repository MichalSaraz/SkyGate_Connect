using System.Linq.Expressions;
using Core.PassengerContext.APIS;

namespace Core.Interfaces
{
    public interface IAPISDataRepository : IGenericRepository<APISData>
    {
        /// <summary>
        /// Retrieves APISData from the repository based on the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria used to filter the APISData.</param>
        /// <param name="tracked">Determines whether the retrieved APISData should be tracked by the repository context.
        /// Default is true.</param>
        /// <returns>The first APISData that satisfies the specified criteria, or null if no match is found.</returns>
        Task<APISData> GetAPISDataByCriteriaAsync(Expression<Func<APISData, bool>> criteria, bool tracked = true);
    }
}