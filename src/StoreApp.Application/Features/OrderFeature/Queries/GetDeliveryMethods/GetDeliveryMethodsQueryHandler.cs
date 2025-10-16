using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.OrderDto;
using StoreApp.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.OrderFeature.Queries.GetDeliveryMethods
{
    public class GetDeliveryMethodsQuery : IRequest<List<DeliveryMethod>>
    {
    }

    public class GetDeliveryMethodsQueryHandler : IRequestHandler<GetDeliveryMethodsQuery, List<DeliveryMethod>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetDeliveryMethodsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<DeliveryMethod>> Handle(GetDeliveryMethodsQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Repository<DeliveryMethod>().ToListAsync(cancellationToken);
        }
    }
}
