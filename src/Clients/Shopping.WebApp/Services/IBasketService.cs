namespace Shopping.WebApp.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBasketService
{
    Task AddAsync(Guid buyerId, Guid productId);
    Task<IReadOnlyCollection<Models.BasketItem>> GetAsync(Guid buyerId);
    Task DeleteAsync(Guid buyerId);
}