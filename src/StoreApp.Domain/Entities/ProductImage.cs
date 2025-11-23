using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities
{
    public class ProductImage : BaseAuditableEntity
    {
        public string ImageUrl { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
