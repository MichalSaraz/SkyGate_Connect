using Core.FlightContext;
using Core.FlightContext.FlightInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFlightRepository
    {
        Task<Flight> GetFlightByCriteriaAsync(Expression<Func<Flight, bool>> criteria, bool tracked = false);
        Task<IReadOnlyList<Flight>> GetFlightsByCriteriaAsync(Expression<Func<Flight, bool>> criteria, bool tracked = false);
        Task<Flight> GetFlightByIdAsync(int id, bool tracked = true);
    }
}
