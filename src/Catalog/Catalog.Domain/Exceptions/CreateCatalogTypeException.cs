namespace Catalog.Domain.Exceptions;

public class CreateCatalogTypeException : CatalogDomainException
{
    public CreateCatalogTypeException(string detail) : base($"Could not create catalog type. {detail}")
    {

    }
}
