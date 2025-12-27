using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminProductDto
{
    public class AdminMostSoldProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string BrandName { get; set; }

        public string TypeName { get; set; }

        public string PictureUrl { get; set; }

        public int TotalQuantitySold { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}
