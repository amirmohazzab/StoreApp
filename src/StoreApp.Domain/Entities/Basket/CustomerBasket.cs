using StoreApp.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StoreApp.Domain.Entities.Basket
{
    [Table("CustomerBaskets")]
    public class CustomerBasket
    {
        [Key]
        public string Id { get; set; }

        public int? UserId { get; set; }

        [InverseProperty(nameof(CustomerBasketItem.Basket))]
        public List<CustomerBasketItem> Items { get; set; } = new();

        public decimal CalculateOriginalPrice()
        {
            return Items.Sum(x => x.Price * x.Quantity);
        }

        public DateTime Created { get; set; } = DateTime.Now;

        public string? CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public string? LastModifiedBy { get; set; }

        public bool IsDelete { get; set; }

        [ForeignKey(nameof(UserId))]
        public User.User? User { get; set; }
    }
}
