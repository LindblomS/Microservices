namespace Management.WebApp.Services;

using Catalog.Contracts.Queries;
using Management.WebApp.Models;

public class CatalogService : ICatalogService
{
    public Task CreateAsync(CreateCatalogItem item)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Item>> GetAsync()
    {
        return await Task.FromResult(new[] { new Item("asdf", "asdf", "asdf",10, "type", "brand", 0) });
    }

    public Task UpdateAsync(UpdateCatalogItem item)
    {
        throw new NotImplementedException();
    }
}
