using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IReadOnlyList<Permission>> GetAllAsync();
    }
}
