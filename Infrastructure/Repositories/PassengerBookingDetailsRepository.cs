using Core.Interfaces;
using Core.PassengerContext.Booking;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class PassengerBookingDetailsRepository : GenericRepository<PassengerBookingDetails>,
        IPassengerBookingDetailsRepository
    {
        public PassengerBookingDetailsRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PassengerBookingDetails> GetBookingDetailsByCriteriaAsync(
            Expression<Func<PassengerBookingDetails, bool>> criteria)
        {
            return await _context.PassengerBookingDetails.AsNoTracking()
                .Include(_ => _.PassengerOrItem)
                .Include(_ => _.PNR)
                .Where(criteria)
                .FirstOrDefaultAsync();
        }
    }
}
