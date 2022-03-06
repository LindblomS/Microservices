namespace Ordering.Application.IntegrationEventHandlers;

using Catalog.Contracts.IntegrationEvents;
using Commands;
using EventBus.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public class OrderStockConfirmedIntegrationEventHandler : BaseIntegrationHandler, IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>
{
    readonly IMediator mediator;

    public OrderStockConfirmedIntegrationEventHandler(IMediator mediator, ILogger<OrderStockConfirmedIntegrationEventHandler> logger)
        : base(logger)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Handle(OrderStockConfirmedIntegrationEvent @event)
    {
        await Handle(async () => await mediator.Send(new SetStockConfirmedOrderStatus(@event.OrderId)), @event);
    }
}
