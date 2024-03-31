using Core.FlightContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IOtherFlightRepository : IBaseFlightRepository
    {
        Task<OtherFlight> GetOtherFlightByCriteriaAsync(Expression<Func<OtherFlight, bool>> criteria, bool tracked = false);
    }
}