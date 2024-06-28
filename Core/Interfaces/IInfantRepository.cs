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

        Task<IReadOnlyList<Infant>> GetInfantsByCriteriaAsync(Expression<Func<Infant, bool>> criteria,
            bool tracked = false);
    }
}