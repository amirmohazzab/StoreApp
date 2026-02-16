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
        public string? DisplayName { get; set; }

        public string? NationalCode { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; } = new List<UserRole>();

        public ICollection<UserLike> UserLikes { get; set; } = new List<UserLike>();

        public bool IsActive { get; set; } = true;

        public string? MainRole { get; set; }

        public string? AvatarUrl { get; set; }

        public string? AvatarPublicId { get; set; }

        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

        public ICollection<UserWishlist> UserWishlists { get; set; } = new List<UserWishlist>();
    }
}
