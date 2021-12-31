namespace Basket.Infrastructure.Exceptions;

using System;

public class BasketRepositoryException : Exception
{
    public BasketRepositoryException(string message) : base(message)
    {
    }
}
