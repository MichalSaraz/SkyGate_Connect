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
    public interface IFlightRepository<T> : IBaseFlightRepository<T> where T : BaseFlight
    {
        Task<TFlight> GetFlightByCriteriaAsync<TFlight>(Expression<Func<TFlight, bool>> criteria, bool tracked = false) where TFlight : BaseFlight;
        Task<IReadOnlyList<Flight>> GetFlightsByCriteriaAsync(Expression<Func<Flight, bool>> criteria, bool tracked = false);
        Task<TFlight> GetFlightByIdAsync<TFlight>(int id, bool tracked = true) where TFlight : BaseFlight;
    }
}
