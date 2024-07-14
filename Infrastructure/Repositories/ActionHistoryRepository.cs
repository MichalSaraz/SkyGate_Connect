using Core.HistoryTracking;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ActionHistoryRepository : GenericRepository<ActionHistory<object>>, IActionHistoryRepository
{
    public  ActionHistoryRepository(AppDbContext context) : base(context)
    {
    }
}