using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string userId);
        Task UpdateAsync(User user);
    }
}
