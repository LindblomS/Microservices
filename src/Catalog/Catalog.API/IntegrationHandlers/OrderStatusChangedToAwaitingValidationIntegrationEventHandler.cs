namespace Catalog.API.IntegrationHandlers;

using EventBus.EventBus.Abstractions;
using Ordering.Contracts.IntegrationEvents;
using System.Threading.Tasks;

public class OrderStatusChangedToAwaitingValidationIntegrationEventHandler : IIntegrationEventHandler<OrderStatusChangedToAwaitingValidationIntegrationEvent>
{
    public Task Handle(OrderStatusChangedToAwaitingValidationIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}
