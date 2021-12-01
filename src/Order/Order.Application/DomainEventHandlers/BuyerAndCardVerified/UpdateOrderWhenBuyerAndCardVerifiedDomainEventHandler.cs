namespace Ordering.Application.DomainEventHandlers.BuyerAndCardVerified;

using MediatR;
using Ordering.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

public class UpdateOrderWhenBuyerAndCardVerifiedDomainEventHandler : INotificationHandler<BuyerAndCardVerifiedDomainEvent>
{
    public Task Handle(BuyerAndCardVerifiedDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
