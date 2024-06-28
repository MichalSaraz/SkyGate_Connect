using Core.PassengerContext.JoinClasses;
using Infrastructure.Data;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class SpecialServiceRequestRepository : GenericRepository<SpecialServiceRequest>,
        ISpecialServiceRequestRepository
    {
        public SpecialServiceRequestRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<SpecialServiceRequest>> GetSpecialServiceRequestsByCriteriaAsync(
            Expression<Func<SpecialServiceRequest, bool>> criteria)
        {
            return await _context.SpecialServiceRequests.AsQueryable().AsNoTracking()
                .Include(_ => _.SSRCode)
                .Include(_ => _.Passenger)
                .Include(_ => _.Passenger.AssignedSeats)
                .Include(_ => _.Passenger.BookingDetails)
                .Where(criteria)
                .ToListAsync();
        }
    }
}