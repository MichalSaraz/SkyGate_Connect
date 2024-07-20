using Core.HistoryTracking;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ActionHistoryRepository : GenericRepository<ActionHistory<object>>, IActionHistoryRepository
{
    public  ActionHistoryRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<IReadOnlyList<ActionHistory<object>>> GetActionHistoryByPassengerId(Guid passengerOrItemId)
    {
        var actionHistoryQuery = _context.ActionHistory.AsQueryable().AsNoTracking()
            .Where(_ => _.PassengerOrItemId == passengerOrItemId);

        var actionHistory = await actionHistoryQuery.ToListAsync();

        return actionHistory;
    }
}