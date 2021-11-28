namespace Services.Order.Domain.Events;

using MediatR;
using Services.Order.Domain.AggregateModels.Order;
using System;
using System.Collections.Generic;

public class OrderStatusChangedToAwaitingValidationDomainEvent : INotification
{
    public OrderStatusChangedToAwaitingValidationDomainEvent(Guid orderId, IEnumerable<OrderItem> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }

    public Guid OrderId { get; }
    public IEnumerable<OrderItem> OrderItems { get; }
}
