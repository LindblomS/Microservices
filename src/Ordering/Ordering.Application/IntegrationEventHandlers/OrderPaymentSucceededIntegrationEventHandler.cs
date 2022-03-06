namespace Ordering.Application.IntegrationEventHandlers;

using Commands;
using EventBus.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Payment.Contracts.IntegrationEvents;
using System;
using System.Threading.Tasks;

public class OrderPaymentSucceededIntegrationEventHandler : BaseIntegrationHandler, IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>
{
    readonly IMediator mediator;

    public OrderPaymentSucceededIntegrationEventHandler(IMediator mediator, ILogger<OrderStockConfirmedIntegrationEventHandler> logger)
        : base(logger)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Handle(OrderPaymentSucceededIntegrationEvent @event)
    {
        await Handle(async () => await mediator.Send(new SetPaidOrderStatus(@event.OrderId)), @event);
    }
}
