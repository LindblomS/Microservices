namespace Ordering.Contracts.IntegrationEvents;

using EventBus.EventBus.Events;
using System;

public class OrderStatusChangedToAwaitingValidationIntegrationEvent : IntegrationEvent
{
    public OrderStatusChangedToAwaitingValidationIntegrationEvent(Guid orderId, IEnumerable<OrderItem> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }

    public Guid OrderId { get; private set; }
    public IEnumerable<OrderItem> OrderItems { get; private set; }

    public record OrderItem(Guid ProductId, int Units);
}
