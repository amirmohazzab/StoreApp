using StoreApp.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Interfaces
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string basketId);

        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket);

        Task<bool> DeleteBasketAsync(string basketId);

        Task<List<CustomerBasket>> GetAllBasketAsync(CancellationToken cancellation);

        Task<CustomerBasket> AddItemToBasketAsync(CustomerBasket basket, CancellationToken cancellationToken);
    }
}
