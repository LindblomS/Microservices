namespace Ordering.Application.IntegrationEventHandlers;

using Catalog.Contracts.IntegrationEvents;
using EventBus.EventBus.Abstractions;
using System;
using System.Threading.Tasks;

public class OrderStockConfirmedIntegrationEventHandler : IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>
{
    public Task Handle(OrderStockConfirmedIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}
