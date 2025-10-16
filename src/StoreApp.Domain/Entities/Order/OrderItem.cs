using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.Order
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
            
        }

        public OrderItem(ProductItemOrdered ItemOrdered, decimal Price, int Quantity)
        {
            this.ItemOrdered = ItemOrdered;
            this.Price = Price;
            this.Quantity = Quantity;
        }

        public ProductItemOrdered ItemOrdered { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
