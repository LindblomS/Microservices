namespace Catalog.Infrastructure.Mappers;

using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.Models;

internal static class CatalogMapper
{
    public static CatalogItem Map(CatalogItemEntity entity)
    {
        return new(
            Map(entity.Id),
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
            Id = Map(item.Id),
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            AvailableStock = item.AvailableStock,
            CatalogTypeId = item.Type.Id,
            CatalogBrandId = item.Brand.Id
        };
    }

    public static CatalogTypeEntity Map(CatalogType type)
    {
        return new CatalogTypeEntity
        {
            Id = type.Id,
            Type = type.Type,
        };
    }

    public static CatalogBrandEntity Map(CatalogBrand brand)
    {
        return new CatalogBrandEntity
        {
            Id = brand.Id,
            Brand = brand.Brand
        };
    }

    static string Map(Guid value)
    {
        return value == default ? null : value.ToString();
    }

    static Guid Map(string value)
    {
        return value is null ? Guid.Empty : Guid.Parse(value);
    }
}
