namespace Ordering.Application.IntegrationEventHandlers;

using Catalog.Contracts.IntegrationEvents;
using EventBus.EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using Ordering.Application.Commands;

public class OrderStockRejectedIntegrationEventHandler : BaseIntegrationHandler, IIntegrationEventHandler<OrderStockRejectedIntegrationEvent>
{
    readonly IMediator mediator;

    public OrderStockRejectedIntegrationEventHandler(IMediator mediator, ILogger<OrderStockRejectedIntegrationEventHandler> logger)
        : base(logger)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Handle(OrderStockRejectedIntegrationEvent @event)
    {
        await Handle(async () =>
        {
            var rejectedItems = @event.OrderItems
                .Where(x => !x.HasStock)
                .Select(x => x.ProductId);

            _ = await mediator.Send(new SetStockRejectedOrderStatus(@event.OrderId, rejectedItems));
        }, @event);
    }
}
