using MediatR;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.BasketFeature.Commands.AddToBasket
{
    public class AddToBasketCommand : IRequest<CustomerBasket>
    {
        public AddToBasketCommand(CustomerBasket customerBasket)
        {
            CustomerBasket = customerBasket;
        }

        public CustomerBasket CustomerBasket { get; set; }
    }

    public class AddToBasketCommandHandler : IRequestHandler<AddToBasketCommand, CustomerBasket>
    {
        private readonly IBasketRepository basketRepository;

        public AddToBasketCommandHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        public async Task<CustomerBasket> Handle(AddToBasketCommand request, CancellationToken cancellationToken)
        {
            return await basketRepository.AddItemToBasketAsync(request.CustomerBasket, cancellationToken);
        }
    }
}
