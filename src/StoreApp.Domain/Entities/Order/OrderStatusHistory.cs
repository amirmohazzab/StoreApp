using StoreApp.Domain.Entities.Base;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.Order
{
    internal class OrderStatusHistory :  BaseEntity
    {
        public int OrderId { get; set; }

        public OrderStatus PreviousStatus { get; set; }

        public OrderStatus NewStatus { get; set; }

        public string ChangedBy { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; } = DateTime.Now;

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
    }
}
