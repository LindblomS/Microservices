namespace Catalog.API.Mappers;

using Catalog.API.Models;
using Catalog.Infrastructure.Models;

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

    public static CatalogItem Map(CatalogItemEntity entity)
    {
        return new CatalogItem
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            AvailableStock = entity.AvailableStock,
            Brand = Map(entity.CatalogBrand),
            Type = Map(entity.CatalogType)
        };
    }

    static CatalogBrand Map(CatalogBrandEntity entity)
    {
        return new CatalogBrand
        {
            Id = entity.Id,
            Brand = entity.Brand
        };
    }

    static CatalogType Map(CatalogTypeEntity entity)
    {
        return new CatalogType
        {
            Id = entity.Id,
            Type = entity.Type
        };
    }
}
