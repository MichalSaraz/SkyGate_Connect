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
        /// Retrieves a baggage based on the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria used to filter the baggage.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the baggage that matches
        /// the specified criteria, or null if no matching baggage is found.
        /// </returns>
        Task<Baggage> GetBaggageByCriteriaAsync(Expression<Func<Baggage, bool>> criteria);

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
