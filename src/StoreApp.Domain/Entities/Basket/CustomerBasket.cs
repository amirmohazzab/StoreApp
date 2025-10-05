using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.Basket
{
    public class CustomerBasket
    {
        public int Id { get; set; }

        public int? UserId { get; set; } 

        public List<CustomerBasketItem> Items { get; set; } = new();
    }
}
