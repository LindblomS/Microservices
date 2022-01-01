namespace Catalog.Infrastructure.Repositories;

using Catalog.Domain.Aggregates;
using Catalog.Infrastructure.EntityFramework;
using System;
using System.Threading.Tasks;

public class CatalogRepository : ICatalogRepository
{
    readonly CatalogContext context;

    public CatalogRepository(CatalogContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task CreateAsync(CatalogItem catalogItem)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(CatalogItem catalogItem)
    {
        throw new NotImplementedException();
    }
}
