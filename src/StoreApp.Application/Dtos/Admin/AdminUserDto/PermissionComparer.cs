using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminUserDto
{
    public class PermissionComparer : IEqualityComparer<UserPermissionDto>
    {
        public bool Equals(UserPermissionDto x, UserPermissionDto y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            return x.Id == y.Id; // یا می‌توانید Name را هم بررسی کنید
        }

        public int GetHashCode(UserPermissionDto obj)
        {
            return obj.Id.GetHashCode(); // یا obj.Name.GetHashCode();
        }
    }
}
