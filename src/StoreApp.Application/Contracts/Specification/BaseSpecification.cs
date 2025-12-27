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

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDesc { get; private set; }
       

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }

        public int Take { get; set; }
        public int Skip { get; set; }

        public bool IsPagingEnabled { get; set; } 

        protected void ApplyPaging(int skip, int take, bool isPagingEnabled = true)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = isPagingEnabled;
        }
    }
}
