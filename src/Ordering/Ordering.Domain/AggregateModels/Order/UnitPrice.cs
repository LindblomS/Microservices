namespace Ordering.Domain.AggregateModels.Order;

using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;

public class UnitPrice : ValueObject
{
    public UnitPrice(decimal price)
    {
        if (price < 1)
            throw new ArgumentException($"Unit price cannot be lower than 1. Unit price was {price}");

        Price = price;
    }

    public decimal Price { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Price;
    }
}
