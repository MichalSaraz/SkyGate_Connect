using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;
            // modify the IQueryable using the specification's criteria expression
            if(spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            // Includes all expression-based includes
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            //// Include any string-based include statements
            //query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
            //// Apply ordering if expressions are set
            //if(spec.OrderBy != null)
            //{
            //    query = query.OrderBy(spec.OrderBy);
            //}
            //else if(spec.OrderByDescending != null)
            //{
            //    query = query.OrderByDescending(spec.OrderByDescending);
            //}
            //// Apply paging if enabled
            //if(spec.IsPagingEnabled)
            //{
            //    query = query.Skip(spec.Skip).Take(spec.Take);
            //}

            return query;
        }
    }
}
