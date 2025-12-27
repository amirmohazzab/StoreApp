using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminOrderDto;
using StoreApp.Domain.Entities.Order;
using StoreApp.Domain.Enums;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminOrderFeature.Commands
{
    public record UpdateOrderStatusCommand(UpdateOrderStatusDto dto) : IRequest<bool>
    {
        
    }

    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Repository<Order>()
                .GetByIdAsync(request.dto.OrderId, cancellationToken);

            if (order == null)
                throw new NotFoundEntityException("Order not found");

            if (!Enum.TryParse<OrderStatus>(request.dto.Status, true, out var statusEnum))
                throw new Exception($"Invalid status value: {request.dto.Status}");

            order.OrderStatus = statusEnum;

            order.IsFinally = statusEnum switch
            {
                OrderStatus.PaymentSuccess => true,
                OrderStatus.Shipped => true,
                OrderStatus.Delivered => true,
                _ => false
            };

            await _unitOfWork.Save(cancellationToken);
            return true;
        }
    }

}
