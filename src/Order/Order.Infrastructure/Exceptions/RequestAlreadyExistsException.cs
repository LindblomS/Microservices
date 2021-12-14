namespace Ordering.Infrastructure.Exceptions;

using System;

public class RequestAlreadyExistsException : Exception
{
    public RequestAlreadyExistsException(Guid requestId) 
        : base($"Request with id {requestId} already exists")
    {

    }
}
