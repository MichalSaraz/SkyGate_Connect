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
        //Task<Passenger> GetPassengerByCriteriaAsync(Expression<Func<Passenger, bool>> criteria, bool tracked = false);
        Task<IReadOnlyList<Passenger>> GetPassengersByCriteriaAsync(Expression<Func<Passenger, bool>> criteria, bool tracked = false);
        Task<Passenger> GetPassengerByIdAsync(Guid id, bool tracked = true, bool displayDetails = false);

        Task<IReadOnlyList<Passenger>> GetPassengersWithFlightConnectionsAsync(int id, bool isOnwardFlight);
    }
}
