using Core.SeatingContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISeatRepository : IGenericRepository<Seat>
    {
        Task<IReadOnlyList<Seat>> GetSeatsByCriteriaAsync(Expression<Func<Seat, bool>> criteria, bool tracked = true);

        Task<Seat> GetSeatByCriteriaAsync(Expression<Func<Seat, bool>> criteria);
    }
}
