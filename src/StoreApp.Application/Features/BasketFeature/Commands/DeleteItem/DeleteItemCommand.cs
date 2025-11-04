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
        public string BasketId { get; set; }

        public int ProductId { get; set; }

        public DeleteItemCommand(string basketId, int productId)
        {
            BasketId = basketId;
            ProductId = productId;
        }
    }
}
