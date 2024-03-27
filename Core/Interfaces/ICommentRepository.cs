using Core.PassengerContext.Booking;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<Comment> GetCommentByIdAsync(Guid id);
        Task<Comment> GetCommentByCriteriaAsync(Expression<Func<Comment, bool>> criteria);
    }
}