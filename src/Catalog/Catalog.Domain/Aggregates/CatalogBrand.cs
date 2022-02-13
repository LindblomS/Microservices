namespace Catalog.Domain.Aggregates;

using Catalog.Domain.Exceptions;

public class CatalogBrand
{
    public CatalogBrand(string brand)
    {
        if (string.IsNullOrWhiteSpace(brand))
            throw new CreateCatalogBrandException("Brand is empty");

        Brand = brand;
    }

    public string Brand { get; private set; }
}

