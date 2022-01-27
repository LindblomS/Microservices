namespace Catalog.Contracts.Queries;

using MediatR;
using System.Collections.Generic;

public record GetItemsQuery(int Id) : IRequest<IEnumerable<Item>>;

public record Item(
    string Id,
    string Name,
    string Description,
    decimal Price,
    Item.CatalogType Type,
    Item.CatalogBrand Brand,
    int AvailableStock)
{
    public record CatalogType(int Id, string Type);

    public record CatalogBrand(int Id, string Brand);
}


