using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities
{
    public class ProductBrand : BaseAuditableEntity, ICommands
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public string Summary { get; set; }

        public User.User User { get; set; }
    }
}
