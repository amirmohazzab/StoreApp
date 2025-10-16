using StoreApp.Domain.Entities.Base;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Domain.Entities.Order
{
    public class Order : BaseAuditableEntity
    {
        public string BuyerPhoneNumber { get; set; }

        public decimal SubTotal { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public string TrackingCode { get; set; }

        public Portal Portal { get; set; }

        public PortalType PortalType { get; set; } = PortalType.Zarrinpal;

        public bool IsFinally { get; set; } = false;

        public string Authority { get; set; }

        public decimal GetOriginalTotal()
        {
            return SubTotal + DeliveryMethod.Price;
        }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public ShipToAddress ShipToAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public User.User User { get; set; }
    }
}
