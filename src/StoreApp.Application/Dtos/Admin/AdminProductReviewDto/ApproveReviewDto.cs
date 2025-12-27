using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminProductReviewDto
{
    public class ApproveReviewDto
    {
        public int ReviewId { get; set; }

        public bool Approve { get; set; }
    }
}
