namespace Ordering.Application.DomainEventHandlers.OrderStatusChangedToPaid;

using MediatR;
using Ordering.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

public class PublishIntegrationEventWhenOrderIsPaidDomainEventHandler : INotificationHandler<OrderStatusChangedToPaidDomainEvent>
{
    public Task Handle(OrderStatusChangedToPaidDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
