using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.Order
{
    public class DeliveryMethod : BaseEntity
    {
        public string ShortName { get; set; }

        public string DeliveryDate { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
