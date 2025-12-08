using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.User
{
    public class Role : IdentityRole<string>
    {
        public ICollection<UserRole>? UserRoles { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
