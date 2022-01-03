namespace Catalog.Domain.Aggregates;

using Catalog.Domain.Exceptions;

public class CatalogBrand
{
    public CatalogBrand(int id, string brand)
    {
        if (id < 1)
            throw new CreateCatalogBrandException($"Id must be greater than 0. Id was {id}");

        if (string.IsNullOrWhiteSpace(brand))
            throw new CreateCatalogBrandException("Brand is empty");

        Id = id;
        Brand = brand;
    }

    public int Id { get; private set; }
    public string Brand { get; private set; }
}

