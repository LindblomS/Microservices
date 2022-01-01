namespace Catalog.Domain.Aggregates;

using Catalog.Domain.Exceptions;

public class CatalogType
{
    public CatalogType(int id, string type)
    {
        if (id < 1)
            throw new CatalogDomainException($"Cannot create catalog type. Id must be greater than 0. Id was {id}");

        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentNullException(nameof(type));

        Id = id;
        Type = type;
    }

    public int Id { get; private set; }
    public string Type { get; private set; }
}
