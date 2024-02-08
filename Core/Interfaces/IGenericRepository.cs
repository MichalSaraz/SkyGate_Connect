using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        //Task<T> GetByIdAsync(int id);
        //Task<IReadOnlyList<T>> GetAllAsync();
        //Task<IReadOnlyList<T>> GetByCriteriaAsync(Expression<Func<T, bool>> criteria);

        //Task<T> GetEntityWithSpec(ISpecification<T> spec);
        //Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec);

        Task<T> AddAsync(params T[] entities);
        Task<T> UpdateAsync(params T[] entities);
        Task<T> DeleteAsync(params T[] entities);
    }
}
