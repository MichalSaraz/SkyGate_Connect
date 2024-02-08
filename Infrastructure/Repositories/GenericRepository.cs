using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {     
            _context = context;
        }

        //public async Task<T> GetByIdAsync(int id)
        //{
        //    return await _context.Set<T>().FindAsync(id);
        //}

        //public async Task<IReadOnlyList<T>> GetAllAsync()
        //{
        //    return await _context.Set<T>().ToListAsync();
        //}

        //public async Task<IReadOnlyList<T>> GetByCriteriaAsync(Expression<Func<T, bool>> criteria)
        //{
        //    return await _context.Set<T>().Where(criteria).ToListAsync();
        //}

        public async Task<T> AddAsync(params T[] entities)
        {
            foreach (var entity in entities)
            {
                await _context.Set<T>().AddAsync(entity);
            }
            await _context.SaveChangesAsync();

            return entities.FirstOrDefault();
        }

        public async Task<T> UpdateAsync(params T[] entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return entities.FirstOrDefault();
        }

        public async Task<T> DeleteAsync(params T[] entities)
        {
            foreach (var entity in entities)
            {
                _context.Set<T>().Remove(entity);
            }

            await _context.SaveChangesAsync();

            return entities.FirstOrDefault();
        }


        //public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        //{
        //    return await ApplySpecification(spec).FirstOrDefaultAsync();
        //}

        //public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
        //{
        //    return await ApplySpecification(spec).ToListAsync();
        //}

        //private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        //{
        //    return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        //}
    }
}
