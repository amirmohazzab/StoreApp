using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminProductDto
{
    public class AdminProductListDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string MainImage { get; set; }

        public string ProductType { get; set; }

        public string ProductBrand { get; set; }

        public string ProductCategory { get; set; }

        public string Colors { get; set; }

        public string Sizes { get; set; }
    }
}
