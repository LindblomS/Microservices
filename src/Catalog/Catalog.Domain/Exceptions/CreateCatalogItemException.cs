namespace Catalog.Domain.Exceptions;

public class CreateCatalogItemException : CatalogDomainException
{
    public CreateCatalogItemException(string detail) : base($"Could not create catalog item. {detail}")
    {

    }
}
