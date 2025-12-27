using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminProductReviewDto
{
    public class MostReviewedProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int ReviewsCount { get; set; }

        public string PictureUrl { get; set; }
    }
}
