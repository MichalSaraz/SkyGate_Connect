using Core.HistoryTracking;

namespace Core.Interfaces;

public interface IActionHistoryRepository : IGenericRepository<ActionHistory<object>>
{
    /// <summary>
    /// Retrieves the action history for a passenger by their ID.
    /// </summary>
    /// <param name="passengerOrItemId">The ID of the passenger or an item associated with the passenger.</param>
    /// <returns>A read-only list of action history records for the specified passenger or item.</returns>
    Task<IReadOnlyList<ActionHistory<object>>> GetActionHistoryByPassengerId(Guid passengerOrItemId);
}