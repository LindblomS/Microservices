namespace Ordering.Application.IntegrationEventHandlers;

using EventBus.EventBus.Abstractions;
using Payment.Contracts.IntegrationEvents;
using System;
using System.Threading.Tasks;
using Commands;
using Serilog.Context;
using MediatR;
using Microsoft.Extensions.Logging;

public class OrderPaymentSucceededIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>
{
    readonly IMediator mediator;
    readonly ILogger<OrderStockConfirmedIntegrationEventHandler> logger;

    public OrderPaymentSucceededIntegrationEventHandler(IMediator mediator, ILogger<OrderStockConfirmedIntegrationEventHandler> logger)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(OrderPaymentSucceededIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEvent", $"{@event.Id}-Ordering"))
        {
            logger.LogInformation("Handling integration event {IntegrationEventId} as Ordering - ({@IntegrationEvent})", @event.Id, @event);

            var command = new SetPaidOrderStatusCommand(@event.OrderId);

            _ = await mediator.Send(command);
        }
    }
}
