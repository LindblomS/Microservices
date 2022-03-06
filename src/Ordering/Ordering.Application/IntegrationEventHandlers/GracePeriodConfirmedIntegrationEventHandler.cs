namespace Ordering.Application.IntegrationEventHandlers;

using EventBus.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Contracts.IntegrationEvents;
using System;
using System.Threading.Tasks;

public class GracePeriodConfirmedIntegrationEventHandler : BaseIntegrationHandler, IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>
{
    readonly IMediator mediator;

    public GracePeriodConfirmedIntegrationEventHandler(
        IMediator mediator,
        ILogger<GracePeriodConfirmedIntegrationEventHandler> logger)
        : base(logger)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Handle(GracePeriodConfirmedIntegrationEvent @event)
    {
        await Handle(async () => await mediator.Send(new SetAwaitingValidationOrderStatus(@event.OrderId)), @event);
    }
}
