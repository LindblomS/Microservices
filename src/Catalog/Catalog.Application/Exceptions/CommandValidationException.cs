namespace Catalog.Application.Exceptions;

using System;
using System.Collections.Generic;

public class CommandValidationException : Exception
{
    public CommandValidationException(IEnumerable<string> validationErrors) : base("Error when validating command")
    {
        ValidationErrors = validationErrors;
    }

    public IEnumerable<string> ValidationErrors { get; private set; }
}
