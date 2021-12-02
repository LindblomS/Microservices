namespace Ordering.Contracts.Exceptions;

using System;

public class BuyerNotFoundException : Exception
{
    public BuyerNotFoundException(Guid buyerId)
        : base($"Buyer with id {buyerId} was not found")
    {
        BuyerId = buyerId;
    }

    public Guid BuyerId { get; private set; }
}
