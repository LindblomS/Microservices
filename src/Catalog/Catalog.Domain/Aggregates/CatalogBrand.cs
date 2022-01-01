namespace Catalog.Domain.Aggregates;

using Catalog.Domain.Exceptions;

public class CatalogBrand
{
    public CatalogBrand(int id, string brand)
    {
        if (id < 1)
            throw new CatalogDomainException($"Cannot create catalog brand. Id must be greater than 0. Id was {id}");

        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentNullException(nameof(brand));

        Id = id;
        Brand = brand;
    }

    public int Id { get; private set; }
    public string Brand { get; private set; }
}

