using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminOrderDetailDto
{
    public class OrderDetailDto
    {
        public int Id { get; set; }

        public string BuyerPhoneNumber { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public string OrderStatus { get; set; }

        public string TrackingCode { get; set; }

        public string Portal { get; set; }

        public string PortalType { get; set; }

        public bool IsFinally { get; set; }

        public string Authority { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();

        public string ShippingAddress { get; set; }

        public decimal DeliveryPrice { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }
    }
}
