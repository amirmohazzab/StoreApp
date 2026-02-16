using AutoMapper;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminProductReviewDto
{
    public class AdminProductReviewDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string UserId { get; set; }

        public string ProductName { get; set; }

        public string UserName { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime Created { get; set; }

        public bool IsApproved { get; set; }

        public FilterReviewStatus Status { get; set; }
    }
}
