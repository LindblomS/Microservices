namespace Ordering.Application.IntegrationEventHandlers;

using Catalog.Contracts.IntegrationEvents;
using EventBus.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Threading.Tasks;
using System.Linq;
using Ordering.Application.Commands;

public class OrderStockRejectedIntegrationEventHandler : IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>
{
    readonly IMediator mediator;
    readonly ILogger<OrderStockRejectedIntegrationEventHandler> logger;

    public OrderStockRejectedIntegrationEventHandler(IMediator mediator, ILogger<OrderStockRejectedIntegrationEventHandler> logger)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderStockRejectedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEvent", $"{@event.Id}-Ordering"))
        {
            logger.LogInformation("Handling integration event {IntegrationEventId} as Ordering - ({@IntegrationEvent})", @event.Id, @event);

            var rejectedItems = @event.OrderItems
                .Where(x => !x.HasStock)
                .Select(x => x.ProductId);

            var command = new SetStockRejectedOrderStatusCommand(@event.OrderId, rejectedItems);

            _ = await mediator.Send(command);
        }
    }
}
