namespace Catalog.Domain.Aggregates;

using Catalog.Domain.Exceptions;

public class CatalogType
{
    public CatalogType(int id, string type)
    {
        if (id < 1)
            throw new CreateCatalogTypeException($"Id must be greater than 0. Id was {id}");

        if (string.IsNullOrWhiteSpace(type))
            throw new CreateCatalogTypeException("Type is empty");

        Id = id;
        Type = type;
    }

    public int Id { get; private set; }
    public string Type { get; private set; }
}
