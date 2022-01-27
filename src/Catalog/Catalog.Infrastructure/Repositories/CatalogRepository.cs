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

    public async Task AddAsync(CatalogItem catalogItem)
    {
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

        return CatalogMapper.Map(item);
    }

    public async Task AddAsync(CatalogBrand brand)
    {
        await context.Brands.AddAsync(CatalogMapper.Map(brand));
    }

    public async Task AddAsync(CatalogType type)
    {
        await context.Types.AddAsync(CatalogMapper.Map(type));
    }

    public Task<CatalogBrand> GetBrandAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<CatalogType> GetTypeAsync(int id)
    {
        throw new NotImplementedException();
    }
}
