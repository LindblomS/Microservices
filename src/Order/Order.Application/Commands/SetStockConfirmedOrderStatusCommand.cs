namespace Ordering.Application.Commands;

using MediatR;

class SetStockConfirmedOrderStatusCommand : IRequest<bool>
{
    public SetStockConfirmedOrderStatusCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
}
