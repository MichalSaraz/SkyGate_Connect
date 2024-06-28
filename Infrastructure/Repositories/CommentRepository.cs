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

        public async Task<Comment> GetCommentByIdAsync(Guid id)
        {
            return await _context.Comments.AsQueryable()
                .Include(_ => _.LinkedToFlights)
                .Where(_ => _.Id == id)
                .FirstOrDefaultAsync();
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
                .Include(_ => _.Passenger.AssignedSeats)
                .Include(_ =>_.Passenger.BookingDetails)
                .Where(criteria)
                .ToListAsync();
        }
    }
}
