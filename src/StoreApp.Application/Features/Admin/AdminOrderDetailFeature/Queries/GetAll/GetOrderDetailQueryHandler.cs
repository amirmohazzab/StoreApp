using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminOrderDetailDto;
using StoreApp.Application.Dtos.Admin.AdminOrderDto;
using StoreApp.Domain.Entities.Order;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminOrderDetailFeature.Queries.GetAll
{
    public record GetOrderDetailQuery(int OrderId) : IRequest<OrderDetailDto>;

    public class GetOrderDetailQueryHandler : IRequestHandler<GetOrderDetailQuery, OrderDetailDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderDetailDto> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
        {

            var order = await _unitOfWork.Repository<Order>()
                    .GetQueryable()
                    .Include(o => o.User)
                    .Include(o => o.DeliveryMethod)
                    .Include(o => o.ShipToAddress)
                    .Include(o => o.OrderItems)
                    .ThenInclude(i => i.ItemOrdered)
                    .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null)
                throw new NotFoundEntityException("Order not found");

            var dto = new OrderDetailDto
            {
                Id = order.Id,
                BuyerPhoneNumber = order.BuyerPhoneNumber,
                SubTotal = order.SubTotal,
                Total = order.GetOriginalTotal(),
                OrderStatus = order.OrderStatus.ToString(),
                //Created = order.Created,

                UserName = order.User?.UserName ?? "-",
                UserEmail = order.User?.Email ?? "-",
                DeliveryPrice = order.DeliveryMethod?.Price ?? 0,
                ShippingAddress = order.ShipToAddress == null
                ? "-"
                : $"{order.ShipToAddress.City} - {order.ShipToAddress.State}",

                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ProductId = i.ItemOrdered.ProductItemId,
                    ProductName = i.ItemOrdered.ProductName,
                    BrandName = i.ItemOrdered.ProductBrandName,
                    TypeName = i.ItemOrdered.ProductTypeName,
                    PictureUrl = i.ItemOrdered.PictureUrl,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            };

            return dto;


            //var data = await query.ToListAsync(cancellationToken);

            //    var result = data.Select(o => new AdminOrderDto
            //    {
            //        Id = o.Id,
            //        BuyerPhoneNumber = o.BuyerPhoneNumber,
            //        SubTotal = o.SubTotal,
            //        Total = o.SubTotal + (o.DeliveryMethod?.Price ?? 0),
            //        OrderStatus = o.OrderStatus,
            //        Created = o.Created,

            //        UserName = o.User?.UserName ?? "-",
            //        UserEmail = o.User?.Email ?? "-",

            //        DeliveryPrice = o.DeliveryMethod?.Price ?? 0,
            //        ShippingAddress = o.ShipToAddress == null
            //? "-"
            //: $"{o.ShipToAddress.City} - {o.ShipToAddress.State}",

            //        Items = o.OrderItems.Select(i => new AdminOrderItemDto
            //        {
            //            ProductId = i.ItemOrdered.ProductItemId,
            //            ProductName = i.ItemOrdered.ProductName,
            //            BrandName = i.ItemOrdered.ProductBrandName,
            //            TypeName = i.ItemOrdered.ProductTypeName,
            //            PictureUrl = i.ItemOrdered.PictureUrl,
            //            Price = i.Price,
            //            Quantity = i.Quantity
            //        }).ToList()
            //    }).ToList();

        }

    }
}
