using MediatR;
using StoreApp.Application.Features.BasketFeature.Queries.GetBasketById;
using StoreApp.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.BasketFeature.Commands.DeleteItem
{
    public class DeleteItemCommand : IRequest<CustomerBasket>
    {
        public int BasketId { get; set; }

        public int ItemId { get; set; }

        public DeleteItemCommand(int basketId, int itemId)
        {
            BasketId = basketId;
            ItemId = itemId;
        }
    }
}
