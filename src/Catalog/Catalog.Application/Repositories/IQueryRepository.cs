namespace Catalog.Application.Repositories;

using Catalog.Contracts.Queries;

public interface IQueryRepository
{
    Task<IEnumerable<Item>> GetItemsAsync();
}
