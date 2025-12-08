using StoreApp.Application.Dtos.Account;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<List<string>> GetUserPermissionsAsync(User user);

        Task<bool> HasPermissionAsync(string userId, string permissionName);

        Task<bool> AddPermissionToRole(string roleId, int permissionId);

        Task<bool> RemovePermissionFromRole(string roleId, int permissionId);

        Task<List<Permission>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Permission?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Permission> CreateAsync(CreatePermissionDto dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(UpdatePermissionDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<List<string>> GetUserPermissionsAsync(User user, CancellationToken cancellationToken = default);
        Task<List<string>> GetRolePermissionsAsync(string roleId, CancellationToken cancellationToken = default);

        Task AssignPermissionToRoleAsync(string roleId, int permissionId, CancellationToken cancellationToken = default);
        Task RemovePermissionFromRoleAsync(string roleId, int permissionId, CancellationToken cancellationToken = default);
        Task<List<string>> GetEffectiveUserPermissionsAsync(string userId);
    }
}
