using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.User
{
    public class UserPermission
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int PermissionId { get; set; }

        public Permission Permission { get; set; }
    }
}
