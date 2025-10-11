using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.User
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }

        public string NationalCode { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
