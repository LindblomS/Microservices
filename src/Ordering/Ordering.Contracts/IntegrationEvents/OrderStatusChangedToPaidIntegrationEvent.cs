namespace Ordering.Contracts.IntegrationEvents;

using EventBus.EventBus.Events;
using System;

public class OrderStatusChangedToPaidIntegrationEvent : IntegrationEvent
{
    public OrderStatusChangedToPaidIntegrationEvent(IEnumerable<OrderItem> orderItems)
    {
        OrderItems = orderItems;
    }

    public IEnumerable<OrderItem> OrderItems { get; private set; }
    public record OrderItem(Guid ProductId, int Units);
}
