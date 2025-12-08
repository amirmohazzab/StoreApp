using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetByIdAsync(string roleId);
        Task<IReadOnlyList<Role>> GetAllAsync();
        Task UpdateAsync(Role role);
    }
}
