namespace Catalog.Domain.Aggregates;

using Catalog.Domain.SeedWork;
using System;
using System.Threading.Tasks;

public interface ICatalogRepository : IRepository<CatalogItem>
{
    Task AddAsync(CatalogItem item);
    Task AddAsync(CatalogBrand brand);
    Task AddAsync(CatalogType type);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(CatalogItem item);
    Task<CatalogItem> GetAsync(Guid id);
    Task<CatalogBrand> GetBrandAsync(int id);
    Task<CatalogType> GetTypeAsync(int id);
}
