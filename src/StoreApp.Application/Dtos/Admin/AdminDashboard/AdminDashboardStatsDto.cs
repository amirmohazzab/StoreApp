using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminDashboard
{
    public class AdminDashboardStatsDto
    {
        public int TotalProducts { get; set; }

        public int TotalUsers { get; set; }

        public int TotalWishlistItems { get; set; }

        public int TotalCategories { get; set; }
    }
}
