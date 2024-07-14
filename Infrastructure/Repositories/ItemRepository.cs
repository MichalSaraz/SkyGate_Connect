using Core.HistoryTracking;
using Core.Interfaces;
using Core.PassengerContext;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ItemRepository : BasePassengerOrItemRepository, IItemRepository
    {
        public ItemRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<bool> ExistsAsync(Guid id)
        {
            var existsInExtraSeat = await _context.Set<ExtraSeat>().AnyAsync(f => f.Id == id);
            var existsInCabinBaggageRequiringSeat = await _context.Set<CabinBaggageRequiringSeat>()
                .AnyAsync(f => f.Id == id);

            return existsInExtraSeat || existsInCabinBaggageRequiringSeat;
        }
    }
}
