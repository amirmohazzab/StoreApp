using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.User
{
    public class UserWishlist : BaseAuditableEntity
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
