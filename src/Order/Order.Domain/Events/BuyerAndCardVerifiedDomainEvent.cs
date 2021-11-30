namespace Ordering.Domain.Events;

using MediatR;
using Ordering.Domain.AggregateModels.Buyer;
using System;

public class BuyerAndCardVerifiedDomainEvent : INotification
{
    public BuyerAndCardVerifiedDomainEvent(Buyer buyer, Card card, Guid orderId)
    {
        Buyer = buyer;
        Card = card;
        OrderId = orderId;
    }

    public Buyer Buyer { get; private set; }
    public Card Card { get; private set; }
    public Guid OrderId { get; private set; }
}
