namespace Catalog.Application.Repositories;

using Catalog.Contracts.Queries;

public interface IQueryRepository
{
    IEnumerable<Item> GetItems();
    Task<Item> GetItemAsync(Guid id);
    IEnumerable<string> GetBrands();
    IEnumerable<string> GetTypes();
}
