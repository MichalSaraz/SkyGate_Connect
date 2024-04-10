using Core.PassengerContext.Booking;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IPassengerBookingDetailsRepository : IGenericRepository<PassengerBookingDetails>
    {
        /// <summary>
        /// Retrieves a passenger booking details based on the given criteria.
        /// </summary>
        /// <param name="criteria">The expression defining the criteria to search for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the passenger booking
        /// details that match the criteria.</returns>
        Task<PassengerBookingDetails> GetBookingDetailsByCriteriaAsync(
            Expression<Func<PassengerBookingDetails, bool>> criteria);
    }
}