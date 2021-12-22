namespace Ordering.Application.IntegrationEventHandlers;

using EventBus.EventBus.Abstractions;
using Payment.Contracts.IntegrationEvents;
using System;
using System.Threading.Tasks;

public class OrderPaymentFailedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
{
    public Task Handle(OrderPaymentFailedIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}
