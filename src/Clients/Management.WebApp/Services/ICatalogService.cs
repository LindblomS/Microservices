namespace Management.WebApp.Services;

using Catalog.Contracts.Queries;
using Management.WebApp.Models;

public interface ICatalogService
{
    Task<IEnumerable<Item>> GetAsync();
    Task CreateAsync(CreateCatalogItem item);
    Task DeleteAsync(string id);
    Task UpdateAsync(UpdateCatalogItem item);
}
