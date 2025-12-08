using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminUserDto
{
    public class AdminUserDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string DisplayName { get; set; }

        public List<UserPermissionDto> Permissions { get; set; } = new();
    }
}
