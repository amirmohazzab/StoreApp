using MediatR;
using StoreApp.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.BasketFeature.Commands.DeleteBasket
{
    public class DeleteBasketCommand : IRequest<bool>
    {
        public DeleteBasketCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
