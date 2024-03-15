using Core.FlightContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IBaseFlightRepository<T> : IGenericRepository<T> where T : BaseFlight
    {
        /// <summary>
        /// Retrieves a flight based on the specified criteria.
        /// </summary>
        /// <typeparam name="T">The type of the flight entity.</typeparam>
        /// <param name="criteria">The criteria used to filter the flights.</param>
        /// <param name="tracked">Indicates whether the flight should be tracked by the context.</param>
        /// <returns>The flight that matches the specified criteria, or null if no flight is found.</returns>
        Task<T> GetFlightByCriteriaAsync(Expression<Func<T, bool>> criteria, bool tracked = false);
    }
}
