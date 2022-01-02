namespace Catalog.API.Mappers;

using Catalog.API.Models;

public static class CatalogMapper
{
    public static Domain.Aggregates.CatalogItem Map(Guid id, CatalogItem item)
    {
        return new(id, item.Name, item.Description, item.Price, Map(item.Type), Map(item.Brand), item.AvailableStock);
    }

    static Domain.Aggregates.CatalogBrand Map(CatalogBrand brand)
    {
        return new(brand.Id, brand.Brand);
    }

    static Domain.Aggregates.CatalogType Map(CatalogType type)
    {
        return new(type.Id, type.Type);
    }
}
