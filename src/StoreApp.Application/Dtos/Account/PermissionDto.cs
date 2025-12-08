using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Account
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string? DisplayName { get; set; }
    }

    public class CreatePermissionDto
    {
        public string Name { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
    }

    public class UpdatePermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
    }
}
