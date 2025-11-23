using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities
{
    public class ProductReview : BaseAuditableEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
        public User.User User { get; set; }

        public int Rating { get; set; } 
        public string Comment { get; set; }
    }
}
