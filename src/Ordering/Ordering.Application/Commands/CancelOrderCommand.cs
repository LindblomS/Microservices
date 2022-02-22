namespace Ordering.Application.Commands;

using MediatR;
using System;

public class CancelOrderCommand : IRequest<bool>
{
    public CancelOrderCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
}