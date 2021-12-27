namespace Ordering.Contracts.IntegrationEvents;

using EventBus.EventBus.Events;
using System;

public class OrderStatusChangedToCancelledIntegrationEvent : IntegrationEvent
{
    public OrderStatusChangedToCancelledIntegrationEvent(Guid orderId, string orderStatus, string buyerName)
    {
        OrderId = orderId;
        OrderStatus = orderStatus;
        BuyerName = buyerName;
    }

    public Guid OrderId { get; private set; }
    public string OrderStatus { get; private set; }
    public string BuyerName { get; private set; }
}
