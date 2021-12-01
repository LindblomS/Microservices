namespace Ordering.Application.DomainEventHandlers.OrderStatusChangedToAwaitingValidation;

using MediatR;
using Ordering.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

public class PublishIntegrationEventWhenOrderIsAwaitingValidationDomainEventHandler : INotificationHandler<OrderStatusChangedToAwaitingValidationDomainEvent>
{
    public Task Handle(OrderStatusChangedToAwaitingValidationDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
