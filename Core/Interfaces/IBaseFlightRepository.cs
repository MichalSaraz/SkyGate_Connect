using Core.FlightContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IBaseFlightRepository : IGenericRepository<BaseFlight>
    {
        Task<BaseFlight> GetFlightByIdAsync(int id, bool tracked = true);
    }
}
