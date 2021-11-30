namespace Ordering.Application.Commands;

using MediatR;
using Ordering.Application.Services;
using Ordering.Domain.AggregateModels.Order;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    readonly IOrderRepository orderRepository;

    public CancelOrderCommandHandler(IOrderRepository orderRepository)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId);

        if (order is null)
            return false;

        order.SetCancelledStatus();
        return await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class CancelOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CancelOrderCommand, bool>
{
    public CancelOrderIdentifiedCommandHandler(
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
