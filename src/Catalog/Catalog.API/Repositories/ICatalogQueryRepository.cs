namespace Catalog.API.Repositories;
using Catalog.API.Models;

public interface ICatalogQueryRepository
{
    IEnumerable<CatalogItem> GetItems();
}
