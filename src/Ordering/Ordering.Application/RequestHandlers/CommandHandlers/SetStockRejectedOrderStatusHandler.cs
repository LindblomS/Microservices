namespace Ordering.Application.RequestHandlers.CommandHandlers;

using MediatR;
using Ordering.Application.Services;
using Ordering.Domain.AggregateModels.Order;
using System.Threading;
using System.Threading.Tasks;
using Ordering.Application.Commands;

public class SetStockRejectedOrderStatusHandler : IRequestHandler<SetStockRejectedOrderStatus, bool>
{
    readonly IOrderRepository orderRepository;

    public SetStockRejectedOrderStatusHandler(IOrderRepository orderRepository)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<bool> Handle(SetStockRejectedOrderStatus request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId);
        order.SetCancelledStatusWhenStockIsRejected(request.StockItems);

        await orderRepository.UpdateAsync(order);

        return true;
    }
}

public class SetStockRejectedOrderStatusIdentifiedCommandHandler : IdentifiedCommandHandler<SetStockRejectedOrderStatus, bool>
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
