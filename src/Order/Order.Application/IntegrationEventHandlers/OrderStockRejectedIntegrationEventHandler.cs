namespace Ordering.Application.IntegrationEventHandlers;

using Catalog.Contracts.IntegrationEvents;
using EventBus.EventBus.Abstractions;
using System;
using System.Threading.Tasks;

public class OrderStockRejectedIntegrationEventHandler : IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>
{
    public Task Handle(OrderStockRejectedIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}
