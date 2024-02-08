using Core.FlightContext.FlightInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IDestinationRepository
    {
        Task<Destination> GetDestinationByCriteriaAsync(Expression<Func<Destination, bool>> criteria);
        Task<IReadOnlyList<Destination>> GetDestinationsByCriteriaAsync(Expression<Func<Destination, bool>> criteria);
    }
}
