using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.OrderDto;
using StoreApp.Application.Features.OrderFeature.Queries.GetOrdersForUser;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.Basket;
using StoreApp.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.BasketFeature.Queries.GetBasketsForUser
{
    public class GetBasketsForUserQuery : IRequest<List<CustomerBasket>>
    {
    }

    public class GetBasketsForUserQueryHandler : IRequestHandler<GetBasketsForUserQuery, List<CustomerBasket>>
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;

        public GetBasketsForUserQueryHandler(IBasketRepository basketRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }

        public async Task<List<CustomerBasket>> Handle(GetBasketsForUserQuery request, CancellationToken cancellationToken)
        {
            return await basketRepository.GetAllBasketAsync(cancellationToken);
        }
    }
}
