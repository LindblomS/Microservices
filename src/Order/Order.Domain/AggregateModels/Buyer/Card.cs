namespace Ordering.Domain.AggregateModels.Buyer;

using Ordering.Domain.SeedWork;
using System;

public class Card : Entity
{
    CardType type;
    string number;
    string securityNumber;
    string holderName;
    DateTime expiration;

    public Card(
        Guid id,
        CardType type,
        string number,
        string securityNumber,
        string holderName,
        DateTime expiration)
    {
        if (id == default)
            throw new ArgumentNullException(nameof(id));

        if (type is null)
            throw new ArgumentNullException(nameof(type));

        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentNullException(nameof(number));

        if (string.IsNullOrWhiteSpace(securityNumber))
            throw new ArgumentNullException(nameof(securityNumber));

        if (string.IsNullOrWhiteSpace(holderName))
            throw new ArgumentNullException(nameof(holderName));

        if (expiration == default)
            throw new ArgumentNullException(nameof(expiration));

        this.id = id;
        this.type = type;
        this.number = number;
        this.securityNumber = securityNumber;
        this.holderName = holderName;
        this.expiration = expiration;
    }

    public CardType Type { get => type; }
    public string Number { get => number; }
    public string SecurityNumber { get => securityNumber; }
    public string HolderName { get => holderName; }
    public DateTime Expiration { get => expiration; }
}
