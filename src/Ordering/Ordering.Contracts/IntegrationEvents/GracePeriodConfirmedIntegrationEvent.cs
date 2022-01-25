namespace Ordering.Contracts.IntegrationEvents;

using EventBus.EventBus.Events;
using System;

public class GracePeriodConfirmedIntegrationEvent : IntegrationEvent
{
    public GracePeriodConfirmedIntegrationEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
}
