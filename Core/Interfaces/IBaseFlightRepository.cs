using Core.FlightContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IBaseFlightRepository : IGenericRepository<BaseFlight>
    {
        Task<IReadOnlyList<BaseFlight>> GetFlightsByCriteriaAsync(
            Expression<Func<BaseFlight, bool>> criteria, bool tracked = false);
        Task<BaseFlight> GetFlightByIdAsync(Guid id, bool tracked = true);
    }
}
