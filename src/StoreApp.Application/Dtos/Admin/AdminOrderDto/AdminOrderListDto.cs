using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminOrderDto
{
    public class AdminOrderListDto
    {
        public int Id { get; set; }

        public string BuyerPhoneNumber { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public bool IsFinally { get; set; }

        public DateTime Created { get; set; }

        public string TrackingCode { get; set; }
    }
}
