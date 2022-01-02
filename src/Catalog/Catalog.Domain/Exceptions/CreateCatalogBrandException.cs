namespace Catalog.Domain.Exceptions;

public class CreateCatalogBrandException : CatalogDomainException
{
    public CreateCatalogBrandException(string detail) : base($"Could not create catalog brand. {detail}")
    {

    }
}
