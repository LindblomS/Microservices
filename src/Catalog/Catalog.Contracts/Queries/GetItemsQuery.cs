namespace Catalog.Contracts.Queries;

using MediatR;
using System.Collections.Generic;

public record GetItemsQuery(int Id) : IRequest<IEnumerable<Item>>;

public record Item(
    string Id,
    string Name,
    string Description,
    decimal Price,
    string Type,
    string Brand,
    int AvailableStock);


