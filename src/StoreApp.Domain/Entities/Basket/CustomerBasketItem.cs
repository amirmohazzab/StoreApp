using StoreApp.Domain.Entities.Base;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.Basket
{
    [Table("CustomerBasketItems")]
    public class CustomerBasketItem : BaseEntity
    {
        public int ProductId { get; set; }

        public string BasketId { get; set; }

        public string ProductName { get; set; }

        public string Type { get; set; }

        public string Brand { get; set; }

        public int Quantity { get; set; }

        public Decimal Price { get; set; }

        public Decimal Discount { get; set; }

        public string PictureUrl { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public CustomerBasket? Basket { get; set; }
    }
}
