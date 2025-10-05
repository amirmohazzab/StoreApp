using MediatR;
using StoreApp.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.BasketFeature.Commands.UpdateBasket
{
    public class UpdateBasketCommand : IRequest<CustomerBasket>
    {
        public UpdateBasketCommand(CustomerBasket customerBasket)
        {
            CustomerBasket = customerBasket;
        }

        public CustomerBasket CustomerBasket { get; set; }
    }
}
