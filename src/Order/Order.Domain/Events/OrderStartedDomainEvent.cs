namespace Services.Order.Domain.Events;

using MediatR;
using System;

public class OrderStartedDomainEvent : INotification
{

    public OrderStartedDomainEvent(
        AggregateModels.Order.Order order,
        Guid userId,
        string username,
        AggregateModels.Order.Card card)
    {
        Order = order;
        UserId = userId;
        UserName = username;
        Card = card;
    }

    public AggregateModels.Order.Order Order { get; }
    public Guid UserId { get; }
    public string UserName { get; }
    public AggregateModels.Order.Card Card { get; }
}
