using Microsoft.EntityFrameworkCore;
using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Contracts.Specification
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery.AsQueryable();

            if (specification.Predicate != null)
            {
                query = query.Where(specification.Predicate);
            }

            if (specification.includes.Any())
            {
                query = specification.includes.Aggregate(query, (current, value) => current.Include(value));
            }

            return query;
        }
    }
}
