using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Interfaces;
using StoreApp.Data.Persistence.Context;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly StoreAppDbContext _context;

        public PermissionRepository(StoreAppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Permission>> GetAllAsync()
        {
            return await _context.Permissions.ToListAsync();
        }
    }
}
