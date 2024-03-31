using Core.FlightContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IFlightRepository : IBaseFlightRepository
    {
        Task<Flight> GetFlightByCriteriaAsync(Expression<Func<Flight, bool>> criteria, bool tracked = false);

        Task<IReadOnlyList<Flight>> GetFlightsByCriteriaAsync(Expression<Func<Flight, bool>> criteria,
            bool tracked = false);        
    }
}