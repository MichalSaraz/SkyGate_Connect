using System.Linq.Expressions;
using Core.PassengerContext.APIS;

namespace Core.Interfaces
{
    public interface ICountryRepository
    {
        /// <summary>
        /// Retrieves a single Country object based on the specified criteria.
        /// </summary>
        /// <param name="criteria">The expression representing the criteria to filter the Country objects.</param>
        /// <returns>The Country object that matches the specified criteria.</returns>
        Task<Country> GetCountryByCriteriaAsync(Expression<Func<Country, bool>> criteria);

        /// <summary>
        /// Retrieves a list of Country objects based on the specified criteria.
        /// </summary>
        /// <param name="criteria">The expression representing the criteria to filter the Country objects.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of Country
        /// objects that match the specified criteria.</returns>
        Task<IReadOnlyList<Country>> GetCountriesByCriteriaAsync(Expression<Func<Country, bool>> criteria);
    }
}