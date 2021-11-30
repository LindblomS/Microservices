namespace Ordering.Domain.AggregateModels.Order;

using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;

public class User : ValueObject
{
    public User(Guid id, string name)
    {
        if (id == default)
            throw new ArgumentNullException(nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        Id = id;
        Name = name;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
        yield return Name;
    }
}
