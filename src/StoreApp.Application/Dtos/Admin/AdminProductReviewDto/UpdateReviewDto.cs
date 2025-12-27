using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminProductReviewDto
{
    public class UpdateReviewDto
    {
        public int ReviewId { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; }
    }
}
