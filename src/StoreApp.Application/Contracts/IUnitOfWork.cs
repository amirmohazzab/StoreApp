using Microsoft.EntityFrameworkCore;
using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Contracts
{
    public interface IUnitOfWork
    {
        DbContext Context { get; }

        Task<int> Save(CancellationToken cancellationToken);

        IGenericRepository<T> Repository<T>() where T : BaseEntity;
    }
}
