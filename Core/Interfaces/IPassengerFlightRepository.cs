namespace Core.Interfaces
{
    public interface IPassengerFlightRepository
    {
        /// <summary>
        /// Gets the highest sequence number of the flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <returns>The highest sequence number of the flight.</returns>
        Task<int> GetHighestSequenceNumberOfTheFlight(Guid flightId);
    }
}
