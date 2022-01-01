namespace Catalog.Infrastructure.Mappers;

using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.Models;

internal static class CatalogMapper
{
    public static CatalogItem Map(CatalogItemEntity entity)
    {
        return new(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Price,
            Map(entity.CatalogType),
            Map(entity.CatalogBrand),
            entity.AvailableStock);
    }

    static CatalogBrand Map(CatalogBrandEntity entity)
    {
        return new(entity.Id, entity.Brand);
    }

    static CatalogType Map(CatalogTypeEntity entity)
    {
        return new(entity.Id, entity.Type);
    }

    public static CatalogItemEntity Map(CatalogItem item)
    {
        return new CatalogItemEntity
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            AvailableStock = item.AvailableStock,
            CatalogTypeId = item.Type.Id,
            CatalogBrandId = item.Brand.Id
        };
    }
}
