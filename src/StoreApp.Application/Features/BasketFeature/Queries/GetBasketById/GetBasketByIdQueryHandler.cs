using MediatR;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.BasketFeature.Queries.GetBasketById
{
    public class GetBasketByIdQueryHandler : IRequestHandler<GetBasketByIdQuery, CustomerBasket>
    {
        private readonly IBasketRepository basketRepository;

        public GetBasketByIdQueryHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository; 
        }

        public async Task<CustomerBasket> Handle(GetBasketByIdQuery request, CancellationToken cancellationToken)
        {
            return await basketRepository.GetBasketAsync(request.Id);
        }
    }
}
