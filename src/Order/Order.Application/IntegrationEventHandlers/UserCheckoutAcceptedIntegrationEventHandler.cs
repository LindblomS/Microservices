namespace Ordering.Application.IntegrationEventHandlers;

using Basket.Contracts.IntegrationEvents;
using EventBus.EventBus.Abstractions;
using System;
using System.Threading.Tasks;

public class UserCheckoutAcceptedIntegrationEventHandler : IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>
{
    public Task Handle(UserCheckoutAcceptedIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}
