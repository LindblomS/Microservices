namespace Ordering.Domain.AggregateModels.Order;

using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;

public class ProductName : ValueObject
{
    public ProductName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
    }

    public string Name { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}
