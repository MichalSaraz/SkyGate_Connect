namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Adds the specified entities to the repository asynchronously.
        /// </summary>
        /// <param name="entities">The entities to be added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<T> AddAsync(params T[] entities);

        /// <summary>
        /// Updates the specified entities in the repository.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <returns>Returns a task that represents the asynchronous operation. The task result contains the updated
        /// entity, or null if no entities were updated.</returns>
        Task<T> UpdateAsync(params T[] entities);

        /// <summary>
        /// Deletes the specified entities from the repository asynchronously.
        /// </summary>
        /// <param name="entities">The entities to be deleted.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the deleted entity,
        /// or null if no entities were deleted.</returns>
        Task<T> DeleteAsync(params T[] entities);
    }
}
