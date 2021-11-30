namespace Order.Application.Commands;

using MediatR;

public class CancelOrderCommand : IRequest<bool>
{
    public CancelOrderCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
}