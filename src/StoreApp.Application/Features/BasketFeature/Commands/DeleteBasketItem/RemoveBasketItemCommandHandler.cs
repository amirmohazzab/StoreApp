using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.BasketFeature.Commands.DeleteBasketItem
{
    public class RemoveBasketItemCommand : IRequest<CustomerBasket>
    {
        public string BasketId { get; set; }
        public int ProductId { get; set; }

        public RemoveBasketItemCommand(string basketId, int productId)
        {
            BasketId = basketId;
            ProductId = productId;
        }
    }

    public class RemoveBasketItemCommandHandler : IRequestHandler<RemoveBasketItemCommand, CustomerBasket>
    {
        private readonly IBasketRepository basketRepository;

        public RemoveBasketItemCommandHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        public async Task<CustomerBasket> Handle(RemoveBasketItemCommand request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetBasketAsync(request.BasketId);
            if (basket == null)
                return null;

            var item = basket.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (item == null)
                return basket; 

            basket.Items.Remove(item);

            var updated = await basketRepository.UpdateBasketAsync(basket);

            return updated;
        }
    }
}

