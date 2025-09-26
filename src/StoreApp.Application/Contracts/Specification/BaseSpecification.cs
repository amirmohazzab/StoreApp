using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Contracts.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Predicate { get; }

        public List<Expression<Func<T, object>>> includes { get; } = new();

        public BaseSpecification()
        {

        }

        public BaseSpecification(Expression<Func<T, bool>> predicate)
        {
            Predicate = predicate;
        }

        protected void AddInclude(Expression<Func<T, object>> include)
        {
            includes.Add(include);
        }
    }
}
