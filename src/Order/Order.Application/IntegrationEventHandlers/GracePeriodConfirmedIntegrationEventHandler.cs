namespace Ordering.Application.IntegrationEventHandlers;
using EventBus.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Contracts.IntegrationEvents;
using Serilog.Context;
using System;
using System.Threading.Tasks;

public class GracePeriodConfirmedIntegrationEventHandler : IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>
{
    readonly IMediator mediator;
    readonly ILogger<GracePeriodConfirmedIntegrationEventHandler> logger;

    public GracePeriodConfirmedIntegrationEventHandler(
        IMediator mediator,
        ILogger<GracePeriodConfirmedIntegrationEventHandler> logger)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

    }

    public async Task Handle(GracePeriodConfirmedIntegrationEvent @event)
    {
        using (LogContext.PushProperty("IntegrationEvent", $"{@event.Id}-Ordering"))
        {
            logger.LogInformation("Handling integration event {IntegrationEventId} as Ordering - ({@IntegrationEvent})", @event.Id, @event);

            var command = new SetAwaitingValidationOrderStatusCommand(@event.OrderId);

            _ = await mediator.Send(command);
        }
    }
}
