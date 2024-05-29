using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;

namespace Core.Interfaces
{
    public interface ICommentService
    {
        /// <summary>
        /// Adds a comment asynchronously.
        /// </summary>
        /// <param name="id">The ID of the comment.</param>
        /// <param name="commentType">The type of the comment.</param>
        /// <param name="text">The text of the comment.</param>
        /// <param name="flightIds">The list of flight IDs associated with the comment.</param>
        /// <param name="predefinedCommentId">The ID of the predefined comment (optional).</param>
        /// <returns>The added comment.</returns>
        Task<Comment> AddCommentAsync(Guid id, CommentTypeEnum commentType, string text, List<Guid> flightIds,
            string predefinedCommentId = null);
    }
}