namespace Ordering.Contracts.IntegrationEvents;

using EventBus.EventBus.Events;
using System;

public class OrderStatusChangedToStockConfirmedIntegrationEvent : IntegrationEvent
{
    public OrderStatusChangedToStockConfirmedIntegrationEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }

}
