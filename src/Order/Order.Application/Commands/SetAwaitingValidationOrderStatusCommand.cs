namespace Ordering.Application.Commands;

using MediatR;

public class SetAwaitingValidationOrderStatusCommand : IRequest<bool>
{
    public SetAwaitingValidationOrderStatusCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
}
