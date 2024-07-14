using Core.BaggageContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IBaggageRepository : IGenericRepository<Baggage>
    {
        /// <summary>
        /// Retrieves a baggage by its tag number.
        /// </summary>
        /// <param name="tagNumber">The tag number of the baggage.</param>
        /// <returns>The baggage with the specified tag number.</returns>
        /// <remarks>
        /// This method retrieves the baggage from the database based on the provided tag number.
        /// It includes related entities such as passenger, baggage tag, special bag, final destination, and flights.
        /// The baggage is retrieved with no tracking to improve performance.
        /// </remarks>
        Task<Baggage> GetBaggageByTagNumber(string tagNumber);


        /// <summary>
        /// Retrieves a baggage item that matches the specified criteria asynchronously.
        /// </summary>
        /// <param name="criteria">The criteria to match against the baggage items.</param>
        /// <param name="tracked">A flag indicating whether the retrieved baggage item should be tracked by the context.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the baggage item that matches the criteria, or null if no match is found.
        /// </returns>
        Task<Baggage> GetBaggageByCriteriaAsync(Expression<Func<Baggage, bool>> criteria, bool tracked = true);

        /// <summary>
        /// Retrieves all baggage that meets the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria to filter the baggage by.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The task result contains a read-only list of baggage that meets the specified criteria.
        /// </returns>
        Task<IReadOnlyList<Baggage>> GetAllBaggageByCriteriaAsync(Expression<Func<Baggage, bool>> criteria);

        /// <summary>
        /// Gets the next value from a sequence.
        /// </summary>
        /// <param name="sequenceName">The name of the sequence.</param>
        /// <returns>The next value from the sequence.</returns>
        int GetNextSequenceValue(string sequenceName);
    }
}
