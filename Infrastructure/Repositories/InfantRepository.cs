using Core.Interfaces;
using Core.PassengerContext;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class InfantRepository : BasePassengerOrItemRepository, IInfantRepository
    {
        public InfantRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Infant> GetInfantByIdAsync(Guid id)
        {
            var infantQuery = _context.Set<Infant>().AsQueryable()
                .Include(_ => _.BookingDetails)
                .Where(_ => _.Id == id);

            return await infantQuery.FirstOrDefaultAsync();
        }
    }
}
