﻿namespace Basket.Infrastructure.Repositories;

using Basket.Domain.AggregateModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RedisBasketRepository : IBasketRepository
{
    public Task CreateUpdateBasketAsync(Basket basket)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBasketAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Guid> GetUsers()
    {
        throw new NotImplementedException();
    }
}