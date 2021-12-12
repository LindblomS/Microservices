namespace Ordering.Application.Commands;

using MediatR;
using Ordering.Application.Exceptions;
using Ordering.Application.Services;
using Ordering.Domain.AggregateModels.Order;
using System.Threading;
using System.Threading.Tasks;

public class SetPaidOrderStatusCommandHandler : IRequestHandler<SetPaidOrderStatusCommand, bool>
{
    readonly IOrderRepository orderRepository;
    readonly DomainEventPublisher domainEventPublisher;

    public SetPaidOrderStatusCommandHandler(IOrderRepository orderRepository, DomainEventPublisher domainEventPublisher)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.domainEventPublisher = domainEventPublisher ?? throw new ArgumentNullException(nameof(domainEventPublisher));
    }

    public async Task<bool> Handle(SetPaidOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId);

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        order.SetPaidStatus();

        await orderRepository.UpdateAsync(order);
        await domainEventPublisher.PublishAsync(order);

        return true;
    }
}

public class SetPaidOrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetPaidOrderStatusCommand, bool>
{
    public SetPaidOrderStatusIdentifiedCommandHandler(
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
