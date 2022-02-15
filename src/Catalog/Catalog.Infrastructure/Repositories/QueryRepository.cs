namespace Catalog.Infrastructure.Repositories;

using Catalog.Application.Repositories;
using Catalog.Contracts.Queries;
using Catalog.Infrastructure.EntityFramework;
using System;
using System.Collections.Generic;

public class QueryRepository : IQueryRepository
{
    readonly CatalogContext context;

    public QueryRepository(CatalogContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IEnumerable<Item> GetItems()
    {
        var entities = context.Items.ToList();

        var items = new List<Item>();

        foreach (var item in entities)
            items.Add(new Item(
                item.Id,
                item.Name,
                item.Description,
                item.Price,
                item.CatalogTypeId,
                item.CatalogBrandId,
                item.AvailableStock));

        return items;
    }

    public IEnumerable<string> GetTypes()
    {
        return context.Types.Select(x => x.Id);
    }

    public IEnumerable<string> GetBrands()
    {
        return context.Brands.Select(x => x.Id);
    }
}
