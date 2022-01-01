namespace Catalog.Domain.Exceptions;

using System;

public class CatalogDomainException : Exception
{
    public CatalogDomainException(string message) : base(message)
    {
    }
}
