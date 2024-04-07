using Core.FlightContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IOtherFlightRepository : IBaseFlightRepository
    {
        /// <summary>
        /// Retrieves an OtherFlight based on the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria expression to filter the OtherFlight.</param>
        /// <param name="tracked">A flag indicating whether to track the OtherFlight entities in the context.</param>
        /// <returns>The first OtherFlight that matches the specified criteria.</returns>
        Task<OtherFlight> GetOtherFlightByCriteriaAsync(Expression<Func<OtherFlight, bool>> criteria,
            bool tracked = false);
    }
}