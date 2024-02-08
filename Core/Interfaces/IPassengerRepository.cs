using Core.PassengerContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPassengerRepository
    {
        Task<Passenger> GetPassengerByCriteriaAsync(Expression<Func<Passenger, bool>> criteria);
        Task<IReadOnlyList<Passenger>> GetPassengersByCriteriaAsync(Expression<Func<Passenger, bool>> criteria);
    }
}
