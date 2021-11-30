namespace Ordering.Domain.AggregateModels.Buyer;

using Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;

public class Card : ValueObject
{
    public Card(
        CardType cardType,
        string cardNumber,
        string cardSecurityNumber,
        string cardHolderName,
        DateTime cardExpiration)
    {
        CardType = cardType;
        CardNumber = cardNumber;
        CardSecurityNumber = cardSecurityNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
    }

    public CardType CardType { get; private set; }
    public string CardNumber { get; private set; }
    public string CardSecurityNumber { get; private set; }
    public string CardHolderName { get; private set; }
    public DateTime CardExpiration { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CardType;
        yield return CardNumber;
        yield return CardExpiration;
    }
}
