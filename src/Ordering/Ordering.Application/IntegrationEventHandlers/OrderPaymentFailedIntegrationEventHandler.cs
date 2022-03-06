namespace Ordering.Application.IntegrationEventHandlers;

using EventBus.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Contracts.IntegrationEvents;
using System;
using System.Threading.Tasks;
using Commands;

public class OrderPaymentFailedIntegrationEventHandler : BaseIntegrationHandler, IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
{
    readonly IMediator mediator;

    public OrderPaymentFailedIntegrationEventHandler(IMediator mediator, ILogger<OrderPaymentFailedIntegrationEventHandler> logger)
        : base(logger)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Handle(OrderPaymentFailedIntegrationEvent @event)
    {
        await Handle(async () => await mediator.Send(new CancelOrder(@event.OrderId)), @event);
    }
}
