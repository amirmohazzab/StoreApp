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

namespace StoreApp.Application.Features.OrderFeature.Queries.GetOrderByIdForUser
{
    public class GetOrderByIdForUserQuery : IRequest<OrderDto>
    {
        public int Id { get; set; }

        public GetOrderByIdForUserQuery(int id)
        {
            Id = id;
        }
    }

    public class GetOrderByIdForUserQueryHandler : IRequestHandler<GetOrderByIdForUserQuery, OrderDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;

        public GetOrderByIdForUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }

        public async Task<OrderDto> Handle(GetOrderByIdForUserQuery request, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Repository<Order>()
                .Where(o => o.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            return mapper.Map<OrderDto>(order);
        }
    }
}
