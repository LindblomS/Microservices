namespace Ordering.Domain.AggregateModels.Order;

using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;

public class Card : ValueObject
{
    public Card(
        int typeId,
        string number,
        string securityNumber,
        string holderName,
        DateTime expiration)
    {
        TypeId = typeId;
        Number = number;
        SecurityNumber = securityNumber;
        HolderName = holderName;
        Expiration = expiration;
    }

    public int TypeId { get; private set; }
    public string Number { get; private set; }
    public string SecurityNumber { get; private set; }
    public string HolderName { get; private set; }
    public DateTime Expiration { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TypeId;
        yield return Number;
        yield return SecurityNumber;
        yield return HolderName;
        yield return Expiration;
    }
}
