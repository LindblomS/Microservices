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
            MapType(entity.CatalogTypeId),
            MapBrand(entity.CatalogBrandId),
            entity.AvailableStock);
    }

    public static CatalogBrand MapBrand(string id)
    {
        return new(id);
    }

    public static CatalogType MapType(string id)
    {
        return new(id);
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
            CatalogTypeId = item.Type.Type,
            CatalogBrandId = item.Brand.Brand
        };
    }

    public static CatalogTypeEntity Map(CatalogType type)
    {
        return new CatalogTypeEntity
        {
            Id = type.Type,
        };
    }

    public static CatalogBrandEntity Map(CatalogBrand brand)
    {
        return new CatalogBrandEntity
        {
            Id = brand.Brand,
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
