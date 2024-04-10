using Core.PassengerContext.Booking;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IPassengerBookingDetailsRepository : IGenericRepository<PassengerBookingDetails>
    {
        Task<PassengerBookingDetails> GetBookingDetailsByCriteriaAsync(Expression<Func<PassengerBookingDetails, bool>> criteria);
    }
}