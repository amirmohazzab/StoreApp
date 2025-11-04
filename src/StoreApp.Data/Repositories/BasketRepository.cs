using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StoreApp.Application.Contracts;
using StoreApp.Application.Interfaces;
using StoreApp.Data.Persistence.Context;
using StoreApp.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StoreApp.Data.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly StoreAppDbContext dbContext;

        private readonly ICurrentUserService currentUserService;

        public BasketRepository(StoreAppDbContext dbContext, ICurrentUserService currentUserService)
        {
            this.dbContext = dbContext;
            this.currentUserService = currentUserService;
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            var basket = await dbContext.CustomerBaskets
           .Include(b => b.Items)
           .FirstOrDefaultAsync(b => b.Id == basketId);

            if (basket == null) return false;

            dbContext.CustomerBaskets.Remove(basket);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            return await dbContext.CustomerBaskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.Id == basketId);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            CustomerBasket? existingBasket = null;

            if (basket.Id != string.Empty)
            {
                existingBasket = await dbContext.CustomerBaskets
                    .Include(b => b.Items)
                    .FirstOrDefaultAsync(b => b.Id == basket.Id);
            }

            if (existingBasket == null)
            {
                // سبد جدید → ست Navigation Property روی آیتم‌ها
                foreach (var item in basket.Items)
                {
                    item.Basket = basket; // EF Core خودش BasketId را پر می‌کند
                }

                await dbContext.CustomerBaskets.AddAsync(basket);
            }
            else
            {
                // بروزرسانی آیتم‌ها
                var itemsToRemove = existingBasket.Items
                    .Where(oldItem =>
                        basket.Items.All(newItem => newItem.ProductId != oldItem.ProductId) ||
                        basket.Items.Any(newItem => newItem.ProductId == oldItem.ProductId && newItem.Quantity == 0))
                    .ToList();

                dbContext.CustomerBasketItems.RemoveRange(itemsToRemove);

                // بروزرسانی آیتم‌های موجود
                foreach (var existingItem in existingBasket.Items)
                {
                    var updatedItem = basket.Items
                        .FirstOrDefault(i => i.ProductId == existingItem.ProductId && i.Quantity > 0);

                    if (updatedItem != null)
                    {
                        existingItem.Quantity = updatedItem.Quantity;
                        existingItem.Price = updatedItem.Price;
                        existingItem.ProductName = updatedItem.ProductName;
                        existingItem.Discount = updatedItem.Discount;
                        existingItem.Brand = updatedItem.Brand;
                        existingItem.Type = updatedItem.Type;
                        existingItem.PictureUrl = updatedItem.PictureUrl;
                    }
                }

                // اضافه کردن آیتم‌های جدید
                var itemsToAdd = basket.Items
                    .Where(newItem =>
                        newItem.Quantity > 0 &&
                        existingBasket.Items.All(oldItem => oldItem.ProductId != newItem.ProductId))
                    .ToList();

                foreach (var newItem in itemsToAdd)
                {
                    newItem.Basket = existingBasket;
                    existingBasket.Items.Add(newItem);
                }
            }

            await dbContext.SaveChangesAsync();
            return basket;
        }

        public async Task<List<CustomerBasket>> GetAllBasketAsync(CancellationToken cancellation)
        {
            return await dbContext.CustomerBaskets
            .Where(b => b.CreatedBy == currentUserService.UserId)
            .Include(b => b.Items)
            .OrderByDescending(x => x.Created)
            .ToListAsync();
        }

        public async Task<CustomerBasket> AddItemToBasketAsync(CustomerBasket basket, CancellationToken cancellationToken)
        {
            var userIdString = currentUserService.UserId;
            int? userId = null;
            if (int.TryParse(userIdString, out var parsedId))
                userId = parsedId;

            // بررسی وجود سبد در DB
            var existingBasket = await dbContext.CustomerBaskets
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == basket.Id, cancellationToken);

            if (existingBasket == null)
            {
                basket.UserId = userId; // int? برای EF
                basket.CreatedBy = userIdString; // string برای auditing
                dbContext.CustomerBaskets.Add(basket);
            }
            else
            {
                // اضافه کردن آیتم‌های جدید یا بروزرسانی موجود
                foreach (var item in basket.Items)
                {
                    var existItem = existingBasket.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
                    if (existItem != null)
                        existItem.Quantity += item.Quantity;
                    else
                        existingBasket.Items.Add(item);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return basket;
        }
    }
}
