using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.OrderDto;
using StoreApp.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.OrderFeature.Queries.GetOrdersForUser
{
    public class GetOrdersForUserQuery : IRequest<List<OrderDto>>
    {
    }

    public class GetOrdersForUserQueryHandler : IRequestHandler<GetOrdersForUserQuery, List<OrderDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;

        public GetOrdersForUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }
       
        public async Task<List<OrderDto>> Handle(GetOrdersForUserQuery request, CancellationToken cancellationToken)
        {
            var orders = await unitOfWork.Repository<Order>()
                .Where(o => o.CreatedBy == currentUserService.UserId).ToListAsync();

            return mapper.Map<List<OrderDto>>(orders);
        }
    }
}
