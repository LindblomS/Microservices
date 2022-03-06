namespace Ordering.Application.RequestHandlers.CommandHandlers;

using MediatR;
using Ordering.Application.Services;
using Ordering.Domain.AggregateModels.Order;
using System.Threading;
using System.Threading.Tasks;
using Ordering.Application.Commands;

public class SetPaidOrderStatusHandler : IRequestHandler<SetPaidOrderStatus, bool>
{
    readonly IOrderRepository orderRepository;
    readonly DomainEventPublisher domainEventPublisher;

    public SetPaidOrderStatusHandler(IOrderRepository orderRepository, DomainEventPublisher domainEventPublisher)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.domainEventPublisher = domainEventPublisher ?? throw new ArgumentNullException(nameof(domainEventPublisher));
    }

    public async Task<bool> Handle(SetPaidOrderStatus request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId);

        order.SetPaidStatus();

        await orderRepository.UpdateAsync(order);
        await domainEventPublisher.PublishAsync(order);

        return true;
    }
}

public class SetPaidOrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetPaidOrderStatus, bool>
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
