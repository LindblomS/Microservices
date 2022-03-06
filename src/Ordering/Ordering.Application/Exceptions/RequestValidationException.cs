namespace Ordering.Application.Exceptions;

using System;
using System.Collections.Generic;

public class RequestValidationException : Exception
{
    public RequestValidationException(IEnumerable<string> validationErrors) : base()
    {
        ValidatonErrors = validationErrors;
    }

    public IEnumerable<string> ValidatonErrors { get; private set; }
}
