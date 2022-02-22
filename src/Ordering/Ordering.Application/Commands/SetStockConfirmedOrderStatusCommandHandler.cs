namespace Ordering.Application.Commands;

using MediatR;
using Ordering.Application.Services;
using Ordering.Domain.AggregateModels.Order;
using System.Threading;
using System.Threading.Tasks;

public class SetStockConfirmedOrderStatusCommandHandler : IRequestHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    readonly IOrderRepository orderRepository;
    readonly DomainEventPublisher domainEventPublisher;

    public SetStockConfirmedOrderStatusCommandHandler(IOrderRepository orderRepository, DomainEventPublisher domainEventPublisher)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.domainEventPublisher = domainEventPublisher ?? throw new ArgumentNullException(nameof(domainEventPublisher));
    }

    public async Task<bool> Handle(SetStockConfirmedOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId);

        order.SetStockConfirmedStatus();

        await orderRepository.UpdateAsync(order);
        await domainEventPublisher.PublishAsync(order);

        return true;
    }
}
public class SetStockConfirmedOrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetStockConfirmedOrderStatusCommand, bool>
{
    public SetStockConfirmedOrderStatusIdentifiedCommandHandler(
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