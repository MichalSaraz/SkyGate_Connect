using Core.PassengerContext.Booking;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        /// <summary>
        /// Retrieves a comment by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the comment to retrieve.</param>
        /// <returns>The comment with the specified ID, or null if it doesn't exist.</returns>
        Task<Comment> GetCommentByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a comment based on the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria used to filter the comments.</param>
        /// <returns>The first comment that matches the specified criteria.</returns>
        Task<Comment> GetCommentByCriteriaAsync(Expression<Func<Comment, bool>> criteria);


        /// <summary>
        /// Retrieves a list of comments based on the specified criteria asynchronously.
        /// </summary>
        /// <param name="criteria">The criteria used to filter the comments.</param>
        /// <returns>The list of comments that match the specified criteria.</returns>
        Task<IReadOnlyList<Comment>> GetCommentsByCriteriaAsync(Expression<Func<Comment, bool>> criteria);
    }
}