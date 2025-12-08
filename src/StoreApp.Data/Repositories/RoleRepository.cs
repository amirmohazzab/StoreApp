using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
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
    public class RoleRepository : IRoleRepository
    {
        private readonly StoreAppDbContext _context;

        public RoleRepository(StoreAppDbContext context)
        {
            _context = context;
        }

        public async Task<Role> GetByIdAsync(string roleId)
        {
            return await _context.Roles
                .Include(r => r.RolePermissions) // شامل کردن permissionها
                .FirstOrDefaultAsync(r => r.Id == roleId);
        }

        public async Task<IReadOnlyList<Role>> GetAllAsync()
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                .ToListAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

    }
}
