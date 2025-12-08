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

        public int ProductId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }
        
        public string Comment { get; set; }
        
        public int Rating { get; set; }
        
        public DateTime Created { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductReview, ProductReviewDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.UserName))
            .ForMember(d => d.ProductTitle, o => o.MapFrom(s => s.Product.Title));
        }

        public string ProductTitle { get; set; }
    }
}
