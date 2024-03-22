using Core.PassengerContext.APIS;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public interface ICountryRepository
    {
        Task<Country> GetCountryByCriteriaAsync(Expression<Func<Country, bool>> criteria);

        Task<IReadOnlyList<Country>> GetCountriesByCriteriaAsync(Expression<Func<Country, bool>> criteria);
    }
}