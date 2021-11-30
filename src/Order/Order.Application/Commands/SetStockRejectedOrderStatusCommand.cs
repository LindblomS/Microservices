namespace Ordering.Application.Commands;

using MediatR;

class SetStockRejectedOrderStatusCommand : IRequest<bool>
{
    public SetStockRejectedOrderStatusCommand(Guid orderId, IEnumerable<int> stockItems)
    {
        OrderId = orderId;
        StockItems = stockItems;
    }

    public Guid OrderId { get; private set; }
    public IEnumerable<int> StockItems { get; private set; }
}
