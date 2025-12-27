using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminOrderDto
{
    public class UpdateOrderStatusDto
    {
        public int OrderId { get; set; }

        public string Status { get; set; }
    }
}
