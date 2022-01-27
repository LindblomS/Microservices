namespace Catalog.Domain.Aggregates;

using Catalog.Domain.Exceptions;

public class CatalogType
{
    public CatalogType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new CreateCatalogTypeException("Type is empty");

        Type = type;
    }

    public string Type { get; private set; }
}
