using Core.Dtos;
using Core.FlightContext;
using Core.PassengerContext;

namespace Core.Interfaces;

public interface IPassengerHistoryService
{
    /// <summary>
    /// Saves the passenger actions to the passenger history asynchronously.
    /// </summary>
    /// <param name="oldValues">The old values.</param>
    /// <param name="passengerOrItems">The passenger or items.</param>
    /// <param name="selectedFlight">The selected flight.</param>
    /// <param name="displayDetails">If set to <c>true</c>, display details.</param>
    /// <returns>Returns a task of type List&lt;object&gt; representing the passenger actions saved to the passenger
    /// history.</returns>
    Task<List<object>> SavePassengerOrItemActionsToPassengerHistoryAsync(IReadOnlyList<BasePassengerOrItem> oldValues,
        IReadOnlyList<BasePassengerOrItem> passengerOrItems, BaseFlight selectedFlight, bool displayDetails = true);

    /// <summary>
    /// Saves the seat actions to the passenger history asynchronously.
    /// </summary>
    /// <param name="flightId">The flight ID.</param>
    /// <param name="oldValues">The list of old passenger or item values.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<List<SeatDto>> SaveSeatActionsToPassengerHistoryAsync(Guid flightId,
        IReadOnlyList<BasePassengerOrItem> oldValues);
}