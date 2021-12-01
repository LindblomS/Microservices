namespace Ordering.Contracts.Exceptions;

using System;

public class OrderNotFoundException : Exception
{
    public OrderNotFoundException(Guid orderId)
        : base($"Order with Id {orderId} was not found")
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; private set; }
}
