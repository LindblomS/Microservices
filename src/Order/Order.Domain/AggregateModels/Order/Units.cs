namespace Services.Order.Domain.AggregateModels.Order;

using Services.Order.Domain.Exceptions;
using Services.Order.Domain.SeedWork;
using System;
using System.Collections.Generic;

public class Units : ValueObject
{
    public Units(int units)
    {
        if (units < 0)
            throw new ArgumentException($"Invalid units. Units must be greater than 0. Units was {units}");

        Value = units;
    }

    public int Value { get; private set; }

    public void AddUnits(int units)
    {
        if (units < 1)
            throw new OrderingDomainException($"Invalid units. Units must be greater than 0. Units was {units}");

        Value += units;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
