namespace Catalog.Application.Repositories;

using Catalog.Contracts.Queries;

public interface IQueryRepository
{
    IEnumerable<Item> GetItems();
    IEnumerable<string> GetBrands();
    IEnumerable<string> GetTypes();
}
