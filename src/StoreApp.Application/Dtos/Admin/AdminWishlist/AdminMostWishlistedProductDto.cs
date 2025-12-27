using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminWishlist
{
    public class AdminMostWishlistedProductDto
    {
        //public string UserId { get; set; }

        //public string UserName { get; set; }

        public int ProductId { get; set; }

        public string ProductTitle { get; set; }

        //public decimal Price { get; set; }

        //public string DisplayName { get; set; }

        public int Wishcount { get; set; }

        public string PictureUrl { get; set; }

        public string ProductBrand { get; set; }
    }
}
