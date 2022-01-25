namespace Ordering.Contracts.IntegrationEvents;

using EventBus.EventBus.Events;
using System;

public class OrderStartedIntegrationEvent : IntegrationEvent
{
    public OrderStartedIntegrationEvent(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; private set; }
}