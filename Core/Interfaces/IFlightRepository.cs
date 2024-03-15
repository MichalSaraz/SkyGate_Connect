using Core.FlightContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IFlightRepository<T> : IBaseFlightRepository<T> where T : BaseFlight
    {
        /// <summary>
        /// Retrieves a flight by a given criteria asynchronously.
        /// </summary>
        /// <typeparam name="TFlight">The type of flight to retrieve.</typeparam>
        /// <param name="criteria">The criteria expression to filter the flights.</param>
        /// <param name="tracked">Specifies whether to track changes in the flight entity.</param>
        /// <returns>A task representing the asynchronous operation, returning the matching flight.</returns>
        Task<TFlight> GetFlightByCriteriaAsync<TFlight>(Expression<Func<TFlight, bool>> criteria, bool tracked = false)
            where TFlight : BaseFlight;

        /// <summary>
        /// Retrieves flights based on the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria to filter the flights.</param>
        /// <param name="tracked">Whether to track the queried flights or not.</param>
        /// <returns> A task that represents the asynchronous operation. The task result contains a read-only list of
        /// flights that match the specified criteria. </returns>
        Task<IReadOnlyList<Flight>> GetFlightsByCriteriaAsync(Expression<Func<Flight, bool>> criteria,
            bool tracked = false);

        /// <summary>
        /// Retrieves a flight by its ID asynchronously.
        /// </summary>
        /// <typeparam name="TFlight">
        /// The type of flight to retrieve. Must inherit from <see cref="BaseFlight"/>. </typeparam>
        /// <param name="id">The ID of the flight to retrieve.</param>
        /// <param name="tracked">
        /// A flag indicating whether the flight should be tracked. Default is <c>true</c>. </param>
        /// <returns> A task that represents the asynchronous operation. The task result contains the flight object
        /// if found, otherwise <c>null</c>. </returns>
        Task<TFlight> GetFlightByIdAsync<TFlight>(int id, bool tracked = true) where TFlight : BaseFlight;
    }
}