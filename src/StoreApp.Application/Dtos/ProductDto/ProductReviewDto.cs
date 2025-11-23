using AutoMapper;
using StoreApp.Application.Common.Mapping;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.ProductDto
{
    public class ProductReviewDto : IMapFrom<ProductReview>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime Created { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductReview, ProductReviewDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName));
        }
    }
}
