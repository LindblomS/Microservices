namespace Catalog.Domain.Aggregates;

using Catalog.Domain.SeedWork;
using System;
using System.Threading.Tasks;

public interface ICatalogRepository : IRepository<CatalogItem>
{
    Task CreateAsync(CatalogItem catalogItem);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(CatalogItem catalogItem);
    Task<CatalogItem> GetAsync(Guid id);
}
