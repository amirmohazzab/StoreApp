using AutoMapper;
using StoreApp.Application.Common.Mapping;
using StoreApp.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.OrderDto
{
    public class OrderItemDto : IMapFrom<OrderItem>
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int ProductItemId { get; set; }

        public string ProductName { get; set; }

        public string ProductBrandName { get; set; }

        public string ProductTypeName { get; set; }

        public string PictureUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrderItem, OrderItemDto>()
                .ForMember(x => x.ProductTypeName,
                    c => c.MapFrom(v => v.ItemOrdered.ProductTypeName))
                .ForMember(x => x.ProductName,
                    c => c.MapFrom(v => v.ItemOrdered.ProductName))
                .ForMember(x => x.ProductItemId,
                    c => c.MapFrom(v => v.ItemOrdered.ProductItemId))
                .ForMember(x => x.ProductBrandName
                    , c => c.MapFrom(v => v.ItemOrdered.ProductBrandName))
                .ForMember(x => x.PictureUrl,
                    c => c.MapFrom(v => v.ItemOrdered.PictureUrl));
        }
    }
}
