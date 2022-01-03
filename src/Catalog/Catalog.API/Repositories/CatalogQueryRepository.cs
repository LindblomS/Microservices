namespace Catalog.API.Repositories;

using Catalog.API.Mappers;
using Catalog.API.Models;
using Catalog.Infrastructure.EntityFramework;

public class CatalogQueryRepository : ICatalogQueryRepository
{
    readonly CatalogContext context;

    public CatalogQueryRepository(CatalogContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<CatalogItem> GetItems()
    {
        var entities = context.Items.ToList();
        var items = new List<CatalogItem>();

        foreach (var item in entities)
            items.Add(CatalogMapper.Map(item));

        return items;
    }
}
