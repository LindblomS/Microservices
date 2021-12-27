namespace Ordering.Application.Exceptions;

using System;

public class UnitOfWorkException : Exception
{
    public UnitOfWorkException(Guid unitOfWorkId, string message, Exception exception) : base(message, exception)
    {
        UnitOfWorkId = unitOfWorkId;
    }

    public Guid UnitOfWorkId { get; private set; }
}
