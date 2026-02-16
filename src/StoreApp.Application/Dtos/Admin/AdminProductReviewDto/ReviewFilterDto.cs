using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminProductReviewDto
{
    public class ReviewFilterDto
    {
        public string? ProductName { get; set; }

        public string? UserName { get; set; }

        public int? Rating { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string? Text { get; set; }

        //public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

        public bool? IsApproved { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public FilterReviewStatus Status { get; set; } = FilterReviewStatus.All;
    }
}
