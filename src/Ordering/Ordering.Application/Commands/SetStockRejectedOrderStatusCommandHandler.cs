namespace Ordering.Application.Commands;

using MediatR;
using Ordering.Application.Services;
using Ordering.Domain.AggregateModels.Order;
using System.Threading;
using System.Threading.Tasks;

public class SetStockRejectedOrderStatusCommandHandler : IRequestHandler<SetStockRejectedOrderStatusCommand, bool>
{
    readonly IOrderRepository orderRepository;

    public SetStockRejectedOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<bool> Handle(SetStockRejectedOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId);
        order.SetCancelledStatusWhenStockIsRejected(request.StockItems);

        await orderRepository.UpdateAsync(order);

        return true;
    }
}

public class SetStockRejectedOrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetStockRejectedOrderStatusCommand, bool>
{
    public SetStockRejectedOrderStatusIdentifiedCommandHandler(
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
