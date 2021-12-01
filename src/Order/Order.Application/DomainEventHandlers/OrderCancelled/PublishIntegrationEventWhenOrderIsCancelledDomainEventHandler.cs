namespace Ordering.Application.DomainEventHandlers.OrderCancelled;

using MediatR;
using Ordering.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

public class PublishIntegrationEventWhenOrderIsCancelledDomainEventHandler : INotificationHandler<OrderCancelledDomainEvent>
{
    public Task Handle(OrderCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
