using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.User
{
    public class UserRole : IdentityUserRole<string>
    {
        public UserRole(string roleId, string userId)
        {
            RoleId = roleId;
            UserId = userId;
        }

        public UserRole()
        {

        }

        
        public User User { get; set; }

        public Role Role { get; set; }
    }
}
