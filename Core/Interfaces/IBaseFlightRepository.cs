using Core.FlightContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IBaseFlightRepository : IGenericRepository<BaseFlight>
    {
        /// <summary>
        /// Retrieves all flights that match the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria used to filter the flights.</param>
        /// <param name="tracked">Indicates whether the fetched flights should be tracked by the context. Default is
        /// false.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of
        /// flights that match the criteria.</returns>
        Task<IReadOnlyList<BaseFlight>> GetFlightsByCriteriaAsync(Expression<Func<BaseFlight, bool>> criteria,
            bool tracked = false);


        /// <summary>
        /// Retrieves a base flight with the specified id asynchronously.
        /// </summary>
        /// <param name="id">The id of the flight.</param>
        /// <param name="tracked">Indicates whether the flight should be tracked by the context. Default is
        /// false.</param>
        /// <param name="displayDetails">Indicates whether additional details of the flight should be displayed.</param>
        /// <returns>The base flight with the specified id.</returns>
        Task<BaseFlight> GetFlightByIdAsync(Guid id, bool tracked = true, bool displayDetails = false);
    }
}
