namespace Services.Order.Domain.AggregateModels.Order;

using Services.Order.Domain.SeedWork;
using System;
using System.Collections.Generic;

public class Card : ValueObject
{
    public Card(
        int cardTypeId,
        string cardNumber,
        string cardSecurityNumber,
        string cardHolderName,
        DateTime cardExpiration)
    {
        CardTypeId = cardTypeId;
        CardNumber = cardNumber;
        CardSecurityNumber = cardSecurityNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
    }

    public int CardTypeId { get; private set; }
    public string CardNumber { get; private set; }
    public string CardSecurityNumber { get; private set; }
    public string CardHolderName { get; private set; }
    public DateTime CardExpiration { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CardTypeId;
        yield return CardNumber;
        yield return CardSecurityNumber;
        yield return CardHolderName;
        yield return CardExpiration;
    }
}
