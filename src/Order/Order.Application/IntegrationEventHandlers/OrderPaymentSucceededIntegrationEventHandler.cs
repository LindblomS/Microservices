namespace Ordering.Application.IntegrationEventHandlers;

using EventBus.EventBus.Abstractions;
using Payment.Contracts.IntegrationEvents;
using System;
using System.Threading.Tasks;

public class OrderPaymentSucceededIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>
{
    public Task Handle(OrderPaymentSucceededIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}
