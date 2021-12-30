namespace Basket.Domain.Exceptions;

using System;

public class BasketDomainException : Exception
{
    public BasketDomainException(string message) : base(message)
    {
    }
}
