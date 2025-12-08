using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.User
{
    public class RolePermission
    {
        public int Id { get; set; }

        public string RoleId { get; set; }

        public Role Role { get; set; }

        public int PermissionId { get; set; }

        public Permission Permission { get; set; }
    }
}
