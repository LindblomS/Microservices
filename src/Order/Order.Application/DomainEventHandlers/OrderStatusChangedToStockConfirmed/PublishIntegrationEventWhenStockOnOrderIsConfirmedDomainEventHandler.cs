namespace Ordering.Application.DomainEventHandlers.OrderStatusChangedToStockConfirmed;

using MediatR;
using Ordering.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

public class PublishIntegrationEventWhenStockOnOrderIsConfirmedDomainEventHandler : INotificationHandler<OrderStatusChangedToStockConfirmedDomainEvent>
{
    public Task Handle(OrderStatusChangedToStockConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
