namespace Ordering.Application.DomainEventHandlers.OrderStarted;

using MediatR;
using Ordering.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

public class AddOrValidateBuyerDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
{
    public Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
