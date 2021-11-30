namespace Order.Application.Commands;

using MediatR;

class CancelOrderCommand : IRequest<bool>
{
    public CancelOrderCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
}