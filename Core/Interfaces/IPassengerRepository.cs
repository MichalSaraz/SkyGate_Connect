using Core.PassengerContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IPassengerRepository : IBasePassengerOrItemRepository
    {
        Task<IReadOnlyList<Passenger>> GetPassengersByCriteriaAsync(Expression<Func<Passenger, bool>> criteria);

        Task<Passenger> GetPassengerByCriteriaAsync(Expression<Func<Passenger, bool>> criteria);

       /// <summary>
       /// Retrieves a passenger by ID asynchronously.
       /// </summary>
       /// <param name="id">The ID of the passenger.</param>
       /// <param name="tracked">Whether to track changes of the retrieved passenger. Default is true.</param>
       /// <param name="displayDetails">
       /// Whether to display additional details of the passenger. Default is false.</param>
       /// <returns> The <see cref="Task"/> representing the asynchronous operation. The task result represents
       /// the passenger with the given ID. </returns>
       Task<Passenger> GetPassengerByIdAsync(Guid id, bool tracked = true, bool displayDetails = false);

        /// <summary>
        /// Retrieves a list of passengers with flight connections based on the flight ID and connection direction.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="isOnwardFlight">Specifies if the flight connection is onward or inbound.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of passengers
        /// with flight connections.</returns>
        Task<IReadOnlyList<Passenger>> GetPassengersWithFlightConnectionsAsync(Guid flightId, bool isOnwardFlight);
    }
}