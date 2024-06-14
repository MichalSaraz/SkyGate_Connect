using Core.PassengerContext.Booking;

namespace Core.Interfaces
{
    public interface IPredefinedCommentRepository
    {
        /// <summary>
        /// Retrieves a predefined comment by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the predefined comment to retrieve.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains
        /// the <see cref="PredefinedComment"/> object if found, otherwise null.</returns>
        Task<PredefinedComment> GetPredefinedCommentByIdAsync(string id);
    }
}