namespace Order.Application.Commands;

using MediatR;

class SetPaidOrderStatusCommand : IRequest<bool>
{
    public SetPaidOrderStatusCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
}
