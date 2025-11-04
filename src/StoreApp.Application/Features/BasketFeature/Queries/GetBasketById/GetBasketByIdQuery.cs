using MediatR;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using StoreApp.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.BasketFeature.Queries.GetBasketById
{
    public class GetBasketByIdQuery : IRequest<CustomerBasket>
    {
        public GetBasketByIdQuery(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
