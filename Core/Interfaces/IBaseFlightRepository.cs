using Core.FlightContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBaseFlightRepository<T> : IGenericRepository<T> where T : BaseFlight
    {
        Task<T> GetFlightByCriteriaAsync(Expression<Func<T, bool>> criteria, bool tracked = false);
    }
}
