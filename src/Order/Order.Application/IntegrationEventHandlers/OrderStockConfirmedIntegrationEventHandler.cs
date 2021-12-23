namespace Ordering.Application.IntegrationEventHandlers;

using Catalog.Contracts.IntegrationEvents;
using EventBus.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Threading.Tasks;
using Commands;

public class OrderStockConfirmedIntegrationEventHandler : IIntegrationEventHandler<OrderStockConfirmedIntegrationEvent>
{
    readonly IMediator mediator;
    readonly ILogger<OrderStockConfirmedIntegrationEventHandler> logger;

    public OrderStockConfirmedIntegrationEventHandler(IMediator mediator, ILogger<OrderStockConfirmedIntegrationEventHandler> logger)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStockConfirmedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEvent", $"{@event.Id}-Ordering"))
        {
            logger.LogInformation("Handling integration event {IntegrationEventId} as Ordering - ({@IntegrationEvent})", @event.Id, @event);

            var command = new SetStockConfirmedOrderStatusCommand(@event.OrderId);

            _ = await mediator.Send(command);
        }
    }
}
