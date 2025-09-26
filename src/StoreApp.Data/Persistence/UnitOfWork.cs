using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Data.Persistence.Context;
using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreAppDbContext dbContext;

        public UnitOfWork(StoreAppDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public DbContext Context => dbContext;

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            return new GenericRepository<T>(dbContext);
        }

        public async Task<int> Save(CancellationToken cancellationToken)
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
