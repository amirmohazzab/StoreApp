using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.User
{
    public class UserLike : BaseEntity
    {
        public string UserId { get; set; }

        public int ProductId { get; set; }

        public bool Liked { get; set; } = true;

        public User User { get; set; }

        public Product? Product { get; set; }
    }
}
