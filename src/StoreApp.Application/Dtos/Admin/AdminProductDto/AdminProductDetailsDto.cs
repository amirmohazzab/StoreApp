using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminProductDto
{
    public class AdminProductDetailsDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public decimal OldPrice { get; set; }

        public string MainImage { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public int ProductTypeId { get; set; }

        public int ProductBrandId { get; set; }

        public int CategoryId { get; set; }

        public List<string> Gallery { get; set; } = new();

        public List<string>? Colors { get; set; }

        public List<string>? Sizes { get; set; }

        public string ProductCategoryName { get; set; }

        public string ProductBrandName { get; set; }

        public string ProductTypeName { get; set; }
    }
}
