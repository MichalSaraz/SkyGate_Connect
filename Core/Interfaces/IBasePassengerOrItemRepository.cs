using Core.PassengerContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IBasePassengerOrItemRepository : IGenericRepository<BasePassengerOrItem>
    {
        /// <summary>
        /// Retrieves a list of BasePassengerOrItem objects that meet the specified criteria.
        /// </summary>
        /// <param name="criteria">The expression representing the criteria to filter the objects.</param>
        /// <param name="tracked">Indicates whether the objects should be tracked by the context.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of
        /// BasePassengerOrItem objects.</returns>
        Task<IReadOnlyList<BasePassengerOrItem>> GetBasePassengerOrItemsByCriteriaAsync(
            Expression<Func<BasePassengerOrItem, bool>> criteria, bool tracked = true);

        /// <summary>
        /// Retrieves a base passenger or item by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the base passenger or item to retrieve.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result contains the base passenger or item with the specified ID,
        /// or null if the base passenger or item is not found.
        /// </returns>
        Task<BasePassengerOrItem> GetBasePassengerOrItemByIdAsync(Guid id);
    }
}