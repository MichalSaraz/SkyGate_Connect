using Core.Interfaces;
using Core.PassengerContext.Booking;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PredefinedCommentRepository : IPredefinedCommentRepository
    {
        private readonly AppDbContext _context;

        public PredefinedCommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PredefinedComment> GetPredefinedCommentByIdAsync(string id)
        {
            return await _context.PredefinedComments.AsNoTracking()
                .Where(_ => _.Id == id)
                .SingleOrDefaultAsync();
        }
    }
}
