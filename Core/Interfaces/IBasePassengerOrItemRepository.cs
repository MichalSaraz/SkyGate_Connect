using Core.PassengerContext;
using System.Linq.Expressions;
using Core.HistoryTracking;

namespace Core.Interfaces
{
    public interface IBasePassengerOrItemRepository : IGenericRepository<BasePassengerOrItem>
    {
        Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// Retrieves a list of BasePassengerOrItem objects that meet the specified criteria.
        /// </summary>
        /// <param name="criteria">The expression representing the criteria to filter the objects.</param>
        /// <param name="tracked">Indicates whether the objects should be tracked by the context. Default is
        /// true.</param>
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

        /// <summary>
        /// Retrieves a BasePassengerOrItem object that meets the specified criteria.
        /// </summary>
        /// <param name="criteria">The expression representing the criteria to filter the object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a BasePassengerOrItem
        /// object that meets the specified criteria, or null if no matching object is found.</returns>
        Task<BasePassengerOrItem> GetBasePassengerOrItemByCriteriaAsync(
            Expression<Func<BasePassengerOrItem, bool>> criteria);

        /// <summary>
        /// Adds an action record to the base passenger or item asynchronously.
        /// </summary>
        /// <param name="basePassengerOrItemId">The ID of the base passenger or item.</param>
        /// <param name="actionHistory">The action history to add.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the base passenger or item
        /// after the action record has been added.
        /// </returns>
        Task<BasePassengerOrItem> AddActionRecordAsync(
            Guid basePassengerOrItemId, ActionHistory<object> actionHistory);
    }
}