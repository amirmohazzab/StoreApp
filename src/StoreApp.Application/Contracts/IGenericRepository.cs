using StoreApp.Application.Contracts.Specification;
using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken);

        Task<T> UpdateAsync(T entity);

        Task Delete(T entity, CancellationToken cancellationToken);

        Task HardDelete(T entity, CancellationToken cancellationToken);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);

        Task<bool> AnyAsync(CancellationToken cancellationToken);

        Task<T> GetEntityWithSpec(ISpecification<T> spec, CancellationToken cancellationToken);

        Task<IReadOnlyList<T>> ListAsyncSpec(ISpecification<T> spec, CancellationToken cancellationToken);

        Task<int> CountAsyncSpec(ISpecification<T> spec, CancellationToken cancellationToken);

        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        Task<List<T>> ToListAsync(CancellationToken cancellationToken);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        void Update(T entity);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);

        IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);

        IQueryable<T> GetQueryable();

    }
}
