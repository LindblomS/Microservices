﻿namespace Ordering.Application.Commands;

using MediatR;

public class SetPaidOrderStatusCommand : IRequest<bool>
{
    public SetPaidOrderStatusCommand(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
}