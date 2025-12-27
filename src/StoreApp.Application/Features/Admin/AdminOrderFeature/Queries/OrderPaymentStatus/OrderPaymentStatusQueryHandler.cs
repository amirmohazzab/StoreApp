using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminOrderDto;
using StoreApp.Domain.Entities.Order;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminOrderFeature.Queries.OrderPaymentStatus
{
    public class OrderPaymentStatusQuery : IRequest<OrderPaymentStatusDto> { }

    public class OrderPaymentStatusQueryHandler : IRequestHandler<OrderPaymentStatusQuery, OrderPaymentStatusDto>
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderPaymentStatusQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<OrderPaymentStatusDto> Handle(OrderPaymentStatusQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Repository<Order>().GetQueryable();

            var paidCount = await query
                .CountAsync(o => o.IsFinally == true, cancellationToken);

            var pendingCount = await query
                .CountAsync(o => o.IsFinally == false && o.OrderStatus == OrderStatus.Pending, cancellationToken);

            return new OrderPaymentStatusDto
            {
                PaidCount = paidCount,
                PendingCount = pendingCount
            };
        }
    }
}
