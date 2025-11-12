using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using StoreApp.Application.Contracts;
using StoreApp.Application.Contracts.Specification;
using StoreApp.Data.Persistence.Context;
using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Persistence
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreAppDbContext dbContext;
        private readonly DbSet<T> dbSet;

        public GenericRepository(StoreAppDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await dbSet.ToListAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await dbSet.AddAsync(entity, cancellationToken);
            return await Task.FromResult(entity);
        }

        public Task<T> UpdateAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(entity);
        }

        public async Task Delete(T entity, CancellationToken cancellationToken)
        {
            var record = await GetByIdAsync(entity.Id, cancellationToken);
            record.IsDelete = true;
            UpdateAsync(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            return await dbSet.AnyAsync(expression, cancellationToken);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return await dbSet.AnyAsync(cancellationToken);
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAsyncSpec(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            return await ApplySpecification(spec).ToListAsync(cancellationToken);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(dbSet.AsQueryable(), spec);
        }

        public async Task<int> CountAsyncSpec(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            return await ApplySpecification(spec).CountAsync(cancellationToken);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression);
        }

        public async Task<List<T>> ToListAsync(CancellationToken cancellationToken)
        {
            return await dbSet.ToListAsync(cancellationToken);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await dbContext.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            return dbSet.Include(navigationPropertyPath);
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return dbSet.Select(selector);
        }

        public IQueryable<T> GetQueryable()
        {
            return dbSet.AsQueryable();
        }
    }
}
