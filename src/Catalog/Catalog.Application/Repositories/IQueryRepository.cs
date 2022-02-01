namespace Catalog.Application.Repositories;

using Catalog.Contracts.Queries;

public interface IQueryRepository
{
    IEnumerable<Item> GetItems();
}
