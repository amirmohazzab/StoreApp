using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.OrderFeature.Queries.GetDeliveryMethodById
{
    public class GetDeliveryMethodByIdQuery : IRequest<DeliveryMethod>
    {
        public int Id { get; set; }

        public GetDeliveryMethodByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetDeliveryMethodByIdQueryHandler : IRequestHandler<GetDeliveryMethodByIdQuery, DeliveryMethod>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetDeliveryMethodByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<DeliveryMethod> Handle(GetDeliveryMethodByIdQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Repository<DeliveryMethod>()
                .Where(o => o.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
