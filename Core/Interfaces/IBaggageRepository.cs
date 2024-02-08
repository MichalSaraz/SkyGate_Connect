using Core.BaggageContext;
using Core.FlightContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBaggageRepository : IGenericRepository<Baggage>
    {
        Task<Baggage> GetBaggageByCriteriaAsync(Expression<Func<Baggage, bool>> criteria);
        Task<IReadOnlyList<Baggage>> GetAllBaggageByCriteriaAsync(Expression<Func<Baggage, bool>> criteria);
        int GetNextSequenceValue(string sequenceName);
    }
}
