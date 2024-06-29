using Core.SeatingContext;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface ISeatRepository : IGenericRepository<Seat>
    {
        /// <summary>
        /// Retrieves a list of seats based on a given criteria.
        /// </summary>
        /// <param name="criteria">The criteria to filter the seats.</param>
        /// <param name="tracked">Indicates whether the seats should be tracked for changes. Defaults is true.</param>
        /// <returns>A read-only list of seats that match the criteria.</returns>
        Task<IReadOnlyList<Seat>> GetSeatsByCriteriaAsync(Expression<Func<Seat, bool>> criteria, bool tracked = true);

        /// <summary>
        /// Retrieves a seat that matches the given criteria.
        /// </summary>
        /// <param name="criteria">The criteria to filter the seat.</param>
        /// <returns>Returns a seat that matches the criteria.</returns>
        Task<Seat> GetSeatByCriteriaAsync(Expression<Func<Seat, bool>> criteria);
    }
}
