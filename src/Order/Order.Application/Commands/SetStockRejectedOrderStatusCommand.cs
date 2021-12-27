namespace Ordering.Application.Commands;

using MediatR;

public class SetStockRejectedOrderStatusCommand : IRequest<bool>
{
    public SetStockRejectedOrderStatusCommand(Guid orderId, IEnumerable<Guid> stockItems)
    {
        OrderId = orderId;
        StockItems = stockItems;
    }

    public Guid OrderId { get; private set; }
    public IEnumerable<Guid> StockItems { get; private set; }
}
