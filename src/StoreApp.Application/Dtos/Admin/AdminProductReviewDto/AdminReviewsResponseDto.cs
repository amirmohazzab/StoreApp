using StoreApp.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminProductReviewDto
{
    public class AdminReviewsResponseDto
    {
        public PaginatedResult<AdminProductReviewDto> Reviews { get; set; }

        public int PendingCount { get; set; }

        public int ApprovedCount { get; set; }

        public int RejectedCount { get; set; }
    }
}
