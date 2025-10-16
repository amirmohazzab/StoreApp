using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.Order;
using StoreApp.Domain.Enums;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZarinpalSandbox;

namespace StoreApp.Application.Features.OrderFeature.Commands.Verify
{
    public class VerifyCommand : IRequest<string>
    {
        public string Authority { get; set; }

        public string Status { get; set; }

        public VerifyCommand(string authority, string status)
        {
            Authority = authority;
            Status = status;
        }
    }

    public class VerifyCommandHandler : IRequestHandler<VerifyCommand, string>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public VerifyCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }

        public async Task<string> Handle(VerifyCommand request, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Context.Set<Order>()
                .Include(o => o.DeliveryMethod)
                .Where(o => o.Authority == request.Authority).SingleOrDefaultAsync(cancellationToken);
            if (order == null) throw new BadRequestEntityException("Order Not Found, Please try again");

            var portal = await unitOfWork.Repository<Portal>()
                .Where(o => o.Id == order.Id).SingleOrDefaultAsync(cancellationToken);
            if (portal == null) throw new BadRequestEntityException("Payment Error, Please Call Support team");

            if (request.Status != "Ok")
            {
                order.OrderStatus = OrderStatus.Cancelled;
                await unitOfWork.Repository<Order>().UpdateAsync(order);

                portal.Status = PaymentDataStatus.Cancelled;
                await unitOfWork.Repository<Portal>().UpdateAsync(portal);
                await unitOfWork.Save(cancellationToken);
                return configuration["Order:CallBackCancelled"];
            };

            var amount = (int)order.GetOriginalTotal();
            var payment = new Payment(amount);
            var result = await payment.Verification(request.Authority);

            if (result.Status == 100)
            {
                order.IsFinally = true;
                order.OrderStatus = OrderStatus.Pending;
                await unitOfWork.Repository<Order>().UpdateAsync(order);
                portal.ReferenceId = result.RefId.ToString();
                portal.Status = PaymentDataStatus.Success;
                await unitOfWork.Repository<Portal>().UpdateAsync(portal);
                await unitOfWork.Save(cancellationToken);

                return configuration["Order:CallBackSuccess"];

            } 
            
            order.OrderStatus = OrderStatus.PaymentFailed;
            await unitOfWork.Repository<Order>().UpdateAsync(order);
            portal.Status = PaymentDataStatus.Failed;
            await unitOfWork.Repository<Portal>().UpdateAsync(portal);
            await unitOfWork.Save(cancellationToken);

           return configuration["Order:CallBackFailed"];
        }
    }
}
