using Core.PassengerContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IInfantRepository : IBasePassengerOrItemRepository
    {
        /// <summary>
        /// Retrieves an <see cref="Infant"/> by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the <see cref="Infant"/>.</param>
        /// <returns>The <see cref="Infant"/> object with the specified ID, or null if not found.</returns>
        Task<Infant> GetInfantByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a list of infants that match the given criteria.
        /// </summary>
        /// <param name="criteria">The criteria by which the infants are filtered.</param>
        /// <param name="tracked">Specifies whether the infants should be tracked by the context. Default is
        /// false.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a read-only list of
        /// infants that match the criteria.</returns>
        Task<IReadOnlyList<Infant>> GetInfantsByCriteriaAsync(Expression<Func<Infant, bool>> criteria,
            bool tracked = false);
    }
}