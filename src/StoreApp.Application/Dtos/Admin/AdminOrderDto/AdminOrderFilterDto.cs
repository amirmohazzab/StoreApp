using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminOrderDto
{
    public class AdminOrderFilterDto
    {
        public string? UserName { get; set; }

        public OrderStatus? Status { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public string? SortBy { get; set; }   // مثلا "Created", "Total", "Status", "UserName"

        public bool SortDesc { get; set; } = false;
    }
}
