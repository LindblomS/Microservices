namespace Ordering.Application.Commands;

using MediatR;

class SetAwaitingValidationOrderStatusCommand : IRequest<bool>
{
    public SetAwaitingValidationOrderStatusCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
}
