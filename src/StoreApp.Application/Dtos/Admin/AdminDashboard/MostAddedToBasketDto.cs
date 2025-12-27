using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminDashboard
{
    public class MostAddedToBasketDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int TotalQuantity { get; set; }

        public string PictureUrl { get; set; }
    }
}
