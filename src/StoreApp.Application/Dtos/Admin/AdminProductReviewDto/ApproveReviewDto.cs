using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminProductReviewDto
{
    public class UpdateReviewStatusDto
    {
        public int ReviewId { get; set; }

        public FilterReviewStatus Status { get; set; }
    }
}
