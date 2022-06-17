namespace Basket.Infrastructure.Repositories;

using Basket.Domain.AggregateModels;
using Basket.Infrastructure.Exceptions;
using Basket.Infrastructure.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

public class RedisBasketRepository : IBasketRepository
{
    readonly ConnectionMultiplexer connection;
    readonly IDatabase database;

    public RedisBasketRepository(ConnectionMultiplexer connection)
    {
        this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        database = connection.GetDatabase();
    }

    public async Task CreateUpdateBasketAsync(Basket basket)
    {
        var set = await database.StringSetAsync(basket.BuyerId.ToString(), JsonSerializer.Serialize(basket));

        if (!set)
            throw new BasketRepositoryException($"Could not create or update basket ({basket.BuyerId})");
    }

    public async Task DeleteBasketAsync(Guid buyerId)
    {
        await database.KeyDeleteAsync(buyerId.ToString());
    }

    public async Task<Basket> GetBasketAsync(Guid buyerId)
    {
        var basket = await database.StringGetAsync(buyerId.ToString());

        if (basket.IsNullOrEmpty)
            return null;

        var dto = JsonSerializer.Deserialize<BasketDto>(basket);
        var domainBasket = new Basket(buyerId);

        foreach (var item in dto.Items)
            domainBasket.AddBasketItem(item);

        return domainBasket;
    }

    public async Task<IEnumerable<Guid>> GetUsersAsync()
    {
        var keys = new List<Guid>();
        var server = GetServer();

        await foreach (var key in server.KeysAsync())
            keys.Add(Guid.Parse(key));

        return keys;
    }

    IServer GetServer()
    {
        return connection.GetServer(connection.GetEndPoints().First());
    }
}
