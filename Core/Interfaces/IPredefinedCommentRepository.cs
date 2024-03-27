using Core.PassengerContext.Booking;

namespace Core.Interfaces
{
    public interface IPredefinedCommentRepository
    {
        Task<PredefinedComment> GetPredefinedCommentByIdAsync(string id);
    }
}