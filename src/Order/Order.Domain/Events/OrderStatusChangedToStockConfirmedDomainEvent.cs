namespace Services.Order.Domain.Events;

using MediatR;
using System;

public class OrderStatusChangedToStockConfirmedDomainEvent : INotification
{
    public OrderStatusChangedToStockConfirmedDomainEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
}
