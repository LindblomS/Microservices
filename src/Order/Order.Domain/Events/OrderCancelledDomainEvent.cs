namespace Ordering.Domain.Events;

using MediatR;
using System;

public class OrderCancelledDomainEvent : INotification
{
    public OrderCancelledDomainEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
}