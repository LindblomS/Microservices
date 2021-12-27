namespace Ordering.Domain.Events;

using MediatR;
using Ordering.Domain.AggregateModels.Order;
using System;
using System.Collections.Generic;

public class OrderStatusChangedToPaidDomainEvent : INotification
{
    public OrderStatusChangedToPaidDomainEvent(Guid orderId, IEnumerable<OrderItem> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }

    public Guid OrderId { get; }
    public IEnumerable<OrderItem> OrderItems { get; }
}
