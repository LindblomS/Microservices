namespace Ordering.Application.Exceptions;

using System;

internal class UnitOfWorkException : Exception
{
    public UnitOfWorkException(Guid unitOfWorkId, string message, Exception exception) : base(message, exception)
    {
        UnitOfWorkId = unitOfWorkId;
    }

    public Guid UnitOfWorkId { get; private set; }
}
