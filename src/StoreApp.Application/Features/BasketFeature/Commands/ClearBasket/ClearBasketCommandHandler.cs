using MediatR;
using StoreApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.BasketFeature.Commands.ClearBasket
{
    public class ClearBasketCommand : IRequest<bool>
    {
        public string BasketId { get; set; }

        public ClearBasketCommand(string basketId)
        {
            BasketId = basketId;
        }
    }

    public class ClearBasketCommandHandler : IRequestHandler<ClearBasketCommand, bool>
    {
        private readonly IBasketRepository basketRepository;

        public ClearBasketCommandHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        public async Task<bool> Handle(ClearBasketCommand request, CancellationToken cancellationToken)
        {
            var basket = await basketRepository.GetBasketAsync(request.BasketId);

            if (basket == null)
                return false;

            basket.Items.Clear();

            await basketRepository.UpdateBasketAsync(basket);

            return true;
        }
    }
}
