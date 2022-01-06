namespace Catalog.Infrastructure.Repositories;

using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.EntityFramework;
using Catalog.Infrastructure.Mappers;
using System;
using System.Threading.Tasks;

public class CatalogRepository : ICatalogRepository
{
    readonly CatalogContext context;

    public CatalogRepository(CatalogContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CreateAsync(CatalogItem catalogItem)
    {
        var type = await context.Types.FindAsync(catalogItem.Type.Id);

        if (type is null)
            await CreateCatalogType(catalogItem.Type);

        var brand = await context.Brands.FindAsync(catalogItem.Brand.Id);

        if (brand is null)
            await CreateCatalogBrand(catalogItem.Brand);

        await context.Items.AddAsync(CatalogMapper.Map(catalogItem));
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var item = await context.Items.FindAsync(id);
        if (item is null)
            return;

        context.Remove(item);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CatalogItem catalogItem)
    {
        var type = await context.Types.FindAsync(catalogItem.Type.Id);

        if (type is null)
            await CreateCatalogType(catalogItem.Type);

        var brand = await context.Brands.FindAsync(catalogItem.Brand.Id);

        if (brand is null)
            await CreateCatalogBrand(catalogItem.Brand);

        var updated = CatalogMapper.Map(catalogItem);
        var original = await context.Items.FindAsync(updated.Id);

        context.Entry(original).CurrentValues.SetValues(updated);
        await context.SaveChangesAsync();
    }

    public async Task<CatalogItem> GetAsync(Guid id)
    {
        var item = await context.Items.FindAsync(id.ToString());

        if (item is null)
            return null;

        item.CatalogBrand = await context.Brands.FindAsync(item.CatalogBrandId);
        item.CatalogType = await context.Types.FindAsync(item.CatalogTypeId);

        return CatalogMapper.Map(item);
    }

    async Task CreateCatalogType(CatalogType type)
    {
        await context.Types.AddAsync(CatalogMapper.Map(type));
    }

    async Task CreateCatalogBrand(CatalogBrand brand)
    {
        await context.Brands.AddAsync(CatalogMapper.Map(brand));
    }

}
