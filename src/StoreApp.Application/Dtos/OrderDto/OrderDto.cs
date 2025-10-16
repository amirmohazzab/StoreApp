using AutoMapper;
using StoreApp.Application.Common.Mapping;
using StoreApp.Application.Dtos.Common;
using StoreApp.Domain.Entities.Order;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.OrderDto
{
    public class OrderDto : BaseAuditableEntityDtoResponse, IMapFrom<Order>
    {
        public string BuyerPhoneNumber { get; set; }

        public decimal SubTotal { get; set; }

        public string TrackingCode { get; set; }

        public bool IsFinally { get; set; } 

        public PortalDto Portal { get; set; }

        public PortalType PortalType { get; set; }

        public string Link { get; set; }

        public string Authority { get; set; }

        public int Status { get; set; }

        public decimal Total { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public ShipToAddress ShipToAddress { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Order, OrderDto>()
                .ForMember(x =>x.Status, c => c.MapFrom(v => (int)v.OrderStatus))
                .ForMember(x => x.Total, c => c.MapFrom(v => v.GetOriginalTotal()));
        }
    }
}
