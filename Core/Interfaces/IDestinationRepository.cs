using Core.FlightContext.FlightInfo;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IDestinationRepository
    {
        /// <summary>
        /// Gets a destination by the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria used to filter the destination.</param>
        /// <returns>The destination that matches the criteria.</returns>
        Task<Destination> GetDestinationByCriteriaAsync(Expression<Func<Destination, bool>> criteria);

        /// <summary>
        /// Retrieves destinations by the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria used to filter the destinations.</param>
        /// <returns>A list of destinations that match the criteria.</returns>
        Task<IReadOnlyList<Destination>> GetDestinationsByCriteriaAsync(Expression<Func<Destination, bool>> criteria);
    }
}
