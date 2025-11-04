using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using StoreApp.Application.Contracts;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.Basket;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.BasketFeature.Commands.DeleteItem
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, CustomerBasket>
    {
        private readonly IBasketRepository basketRepository;

        public DeleteItemCommandHandler(IBasketRepository basketRepositor)
        {
            this.basketRepository = basketRepository;
        }

        public async Task<CustomerBasket> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            //var basket = await basketRepository.GetBasketAsync(request.BasketId);

            //if (basket == null) throw new NotFoundEntityException("Basket Not Found");
            //basket.Items = basket.Items.Where(x => x.ProductId != request.ProductId).ToList();
            //if (basket.Items.Count == 0) 
            //{
            //    await basketRepository.DeleteBasketAsync(request.BasketId);
            //}
            //else
            //{
            //    foreach (var item in basket.Items)
            //    {
            //        item.BasketId = basket.Id;
            //    }
            //    await basketRepository.UpdateBasketAsync(basket);
            //}
            //return basket;

            var basket = await basketRepository.GetBasketAsync(request?.BasketId);

            if (basket == null)
                return null; // 👈 باعث میشه Controller بره به NotFound()

            if (basket.Items == null || !basket.Items.Any())
                return basket;

            basket.Items = basket.Items
                .Where(x => x.ProductId != request.ProductId)
                .ToList();

            if (basket.Items.Count == 0)
            {
                await basketRepository.DeleteBasketAsync(request.BasketId);
                basket.Items = new List<CustomerBasketItem>(); // 👈 برای جلوگیری از NullReference در فرانت
            }
            else
            {
                await basketRepository.UpdateBasketAsync(basket);
            }

            return basket;
        }
    }
}
