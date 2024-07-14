using Core.Interfaces;
using Core.PassengerContext.Booking;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Comment> GetCommentByIdAsync(Guid id, bool tracked = false)
        {
            var commentQuery = _context.Comments.AsQueryable()
                .Include(_ => _.LinkedToFlights)
                    .ThenInclude(_ => _.Flight)
                .Where(_ => _.Id == id);
            
            if (!tracked)
            {
                commentQuery = commentQuery.AsNoTracking();
            }
            
            var comment = await commentQuery.FirstOrDefaultAsync();
            
            return comment;
        }

        public async Task<Comment> GetCommentByCriteriaAsync(Expression<Func<Comment, bool>> criteria)
        {
            return await _context.Comments.AsQueryable()
                .Include(_ => _.LinkedToFlights)
                .Where(criteria)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Comment>> GetCommentsByCriteriaAsync(Expression<Func<Comment, bool>> criteria)
        {
            return await _context.Comments.AsQueryable().AsNoTracking()
                .Include(_ => _.LinkedToFlights)
                    .ThenInclude(_ => _.Flight)
                .Include(_ => _.PassengerOrItem.AssignedSeats)
                .Include(_ =>_.PassengerOrItem.BookingDetails)
                .Where(criteria)
                .ToListAsync();
        }
    }
}
