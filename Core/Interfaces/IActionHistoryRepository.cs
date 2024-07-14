using Core.HistoryTracking;

namespace Core.Interfaces;

public interface IActionHistoryRepository : IGenericRepository<ActionHistory<object>>
{
}