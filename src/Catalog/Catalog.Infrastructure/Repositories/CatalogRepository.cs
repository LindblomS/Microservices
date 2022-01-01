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
        var entity = CatalogMapper.Map(catalogItem);
        context.Update(entity);
        await context.SaveChangesAsync();
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
