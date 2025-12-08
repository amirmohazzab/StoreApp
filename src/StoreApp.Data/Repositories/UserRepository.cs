using StoreApp.Application.Interfaces;
using StoreApp.Data.Persistence.Context;
using StoreApp.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StoreAppDbContext _context;

        public UserRepository(StoreAppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await _context.Users
         .Include(u => u.UserPermissions)
             .ThenInclude(up => up.Permission)
         .Include(u => u.UserRoles)
             .ThenInclude(ur => ur.Role)
                 .ThenInclude(r => r.RolePermissions)
                     .ThenInclude(rp => rp.Permission)
         .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

    }
}
