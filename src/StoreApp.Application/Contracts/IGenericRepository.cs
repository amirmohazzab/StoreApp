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

        Task<T> AddEntity(T entity, CancellationToken cancellationToken);

        Task<T> UpdateEntity(T entity);

        Task Delete(T entity, CancellationToken cancellationToken);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);

        Task<bool> AnyAsync(CancellationToken cancellationToken);

        Task<T> GetEntityWithSpec(ISpecification<T> spec, CancellationToken cancellationToken);

        Task<IReadOnlyList<T>> ListAsyncSpec(ISpecification<T> spec, CancellationToken cancellationToken);
    }
}
