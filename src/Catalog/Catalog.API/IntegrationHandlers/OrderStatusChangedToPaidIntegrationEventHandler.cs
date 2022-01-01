namespace Catalog.API.IntegrationHandlers;

using EventBus.EventBus.Abstractions;
using Ordering.Contracts.IntegrationEvents;
using System.Threading.Tasks;

public class OrderStatusChangedToPaidIntegrationEventHandler : IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    public Task Handle(OrderStatusChangedToPaidIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}
