namespace Basket.Domain.AggregateModels;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.SeedWork;

public interface IBasketRepository : IRepository<Basket>
{
    IEnumerable<Guid> GetUsers();
    Task CreateUpdateBasketAsync(Basket basket);
    Task DeleteBasketAsync(Guid buyerId);
    Task<Basket> GetBasketAsync(Guid buyerId);
}
