namespace Ordering.Application.Commands;

using MediatR;
using Ordering.Application.Services;
using Ordering.Domain.AggregateModels.Order;
using System;
using System.Threading;
using System.Threading.Tasks;

public class SetAwaitingValidationOrderStatusCommandHandler : IRequestHandler<SetAwaitingValidationOrderStatusCommand, bool>
{
    readonly IOrderRepository orderRepository;
    readonly DomainEventPublisher domainEventPublisher;

    public SetAwaitingValidationOrderStatusCommandHandler(IOrderRepository orderRepository, DomainEventPublisher domainEventPublisher)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.domainEventPublisher = domainEventPublisher ?? throw new ArgumentNullException(nameof(domainEventPublisher));
    }

    public async Task<bool> Handle(SetAwaitingValidationOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId);

        order.SetAwaitingValidationStatus();

        await orderRepository.UpdateAsync(order);
        await domainEventPublisher.PublishAsync(order);

        return true;
    }
}

public class SetAwaitingValidationOrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetAwaitingValidationOrderStatusCommand, bool>
{
    public SetAwaitingValidationOrderStatusIdentifiedCommandHandler(
        IRequestManager requestManager,
        IMediator mediator)
        : base(requestManager, mediator)
    {

    }

    protected override bool CreateResultForDuplicateRequest()
    {
        return true;
    }
}
