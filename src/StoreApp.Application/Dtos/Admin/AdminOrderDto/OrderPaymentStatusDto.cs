using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminOrderDto
{
    public class OrderPaymentStatusDto
    {
        public int PaidCount { get; set; }

        public int PendingCount { get; set; }
    }
}
