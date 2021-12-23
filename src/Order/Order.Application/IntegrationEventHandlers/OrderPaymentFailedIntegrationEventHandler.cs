namespace Ordering.Application.IntegrationEventHandlers;

using EventBus.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Contracts.IntegrationEvents;
using System;
using System.Threading.Tasks;
using Commands;
using Serilog.Context;

public class OrderPaymentFailedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
{
    readonly IMediator mediator;
    readonly ILogger<OrderPaymentFailedIntegrationEventHandler> logger;

    public OrderPaymentFailedIntegrationEventHandler(IMediator mediator, ILogger<OrderPaymentFailedIntegrationEventHandler> logger)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderPaymentFailedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEvent", $"{@event.Id}-Ordering"))
        {
            logger.LogInformation("Handling integration event {IntegrationEventId} as Ordering - ({@IntegrationEvent})", @event.Id, @event);

            var command = new CancelOrderCommand(@event.OrderId);

            _ = await mediator.Send(command);
        }
    }
}
