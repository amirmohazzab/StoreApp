using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Dtos.Account;
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
    public class PermissionService : IPermissionService
    {
        private readonly UserManager<User> userManager;
        private readonly StoreAppDbContext db;

        public PermissionService(UserManager<User> userManager, StoreAppDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }

        public async Task<List<string>> GetUserPermissionsAsync(User user)
        {
            var roles = await userManager.GetRolesAsync(user);

            var roleIds = db.Roles
                .Where(r => roles.Contains(r.Name))
                .Select(r => r.Id)
                .ToList();

            return db.RolePermissions
                .Include(rp => rp.Permission)
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Select(rp => rp.Permission.Name)
                .ToList();
        }

        public async Task<bool> HasPermissionAsync(string userId, string permissionName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var roles = await userManager.GetRolesAsync(user);

            var roleIds = db.Roles
                .Where(r => roles.Contains(r.Name))
                .Select(r => r.Id)
                .ToList();

            return db.RolePermissions
                .Include(x => x.Permission)
                .Any(x => roleIds.Contains(x.RoleId) &&
                          x.Permission.Name == permissionName);
        }

        public async Task<bool> AddPermissionToRole(string roleId, int permissionId)
        {
            var exists = await db.RolePermissions.AnyAsync(x =>
                x.RoleId == roleId && x.PermissionId == permissionId);

            if (exists) return false;

            db.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            });

            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePermissionFromRole(string roleId, int permissionId)
        {
            var entity = await db.RolePermissions
                .FirstOrDefaultAsync(x => x.RoleId == roleId && x.PermissionId == permissionId);

            if (entity == null) return false;

            db.RolePermissions.Remove(entity);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await db.Permissions.OrderBy(p => p.Name).ToListAsync(cancellationToken);
        }

        public async Task<Permission?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await db.Permissions.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<Permission> CreateAsync(CreatePermissionDto dto, CancellationToken cancellationToken = default)
        {
            var exists = await db.Permissions.AnyAsync(p => p.Name == dto.Name, cancellationToken);
            if (exists) throw new InvalidOperationException("Permission already exists");

            var p = new Permission { Name = dto.Name, DisplayName = dto.DisplayName };
            db.Permissions.Add(p);
            await db.SaveChangesAsync(cancellationToken);
            return p;
        }

        public async Task<bool> UpdateAsync(UpdatePermissionDto dto, CancellationToken cancellationToken = default)
        {
            var p = await db.Permissions.FindAsync(new object[] { dto.Id }, cancellationToken);
            if (p == null) return false;
            p.Name = dto.Name;
            p.DisplayName = dto.DisplayName;
            db.Permissions.Update(p);
            await db.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var p = await db.Permissions.FindAsync(new object[] { id }, cancellationToken);
            if (p == null) return false;
            // Remove role associations first
            var rps = db.RolePermissions.Where(r => r.PermissionId == id);
            db.RolePermissions.RemoveRange(rps);
            db.Permissions.Remove(p);
            await db.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<List<string>> GetRolePermissionsAsync(string roleId, CancellationToken cancellationToken = default)
        {
            return await db.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task AssignPermissionToRoleAsync(string roleId, int permissionId, CancellationToken cancellationToken = default)
        {
            var exists = await db.RolePermissions.AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);
            if (!exists)
            {
                db.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permissionId });
                await db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task RemovePermissionFromRoleAsync(string roleId, int permissionId, CancellationToken cancellationToken = default)
        {
            var rp = await db.RolePermissions.FirstOrDefaultAsync(x => x.RoleId == roleId && x.PermissionId == permissionId, cancellationToken);
            if (rp != null)
            {
                db.RolePermissions.Remove(rp);
                await db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<List<string>> GetUserPermissionsAsync(User user, CancellationToken cancellationToken = default)
        {
            // get role ids for the user
            var roleIds = await db.UserRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId).ToListAsync(cancellationToken);

            if (!roleIds.Any()) return new List<string>();

            var perms = await db.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Name)
                .ToListAsync(cancellationToken);

            return perms.Distinct().ToList();
        }

        public async Task<List<string>> GetEffectiveUserPermissionsAsync(string userId)
        {
            var user = await db.Users
             .Include(u => u.UserPermissions)
             .Include(u => u.UserRoles)
                 .ThenInclude(ur => ur.Role)
                 .ThenInclude(r => r.RolePermissions)
             .FirstOrDefaultAsync(x => x.Id == userId);

            var userPerms = user.UserPermissions.Select(up => up.Permission.Name);

            var rolePerms = user.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name);

            return userPerms
                .Union(rolePerms)
                .ToList();
        }
    }
}
